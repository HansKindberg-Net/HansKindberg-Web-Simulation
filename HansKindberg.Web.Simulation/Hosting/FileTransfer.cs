using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class FileTransfer : IFileTransfer
    {
        #region Fields

        private readonly DirectoryInfoBase _destinationDirectory;
        private readonly IList<DirectoryInfoBase> _directoriesCreatedOnTransfer = new List<DirectoryInfoBase>();
        private const string _directoryDoesNotExistExceptionMessageFormat = "The directory \"{0}\" does not exist.";
        private const string _fileDoesNotExistExceptionMessageFormat = "The file \"{0}\" does not exist.";
        private readonly IFileSystem _fileSystem;
        private readonly IList<FileInfoBase> _filesCreatedOnTransfer = new List<FileInfoBase>();
        private bool _includeBinaries = true;
        private readonly IList<IFileTransferItem> _items = new List<IFileTransferItem>();
        private const StringComparison _nameComparison = StringComparison.OrdinalIgnoreCase;
        private readonly DirectoryInfoBase _sourceDirectory;

        #endregion

        #region Constructors

        public FileTransfer(IFileSystem fileSystem, string destinationDirectoryPath, string sourceDirectoryPath)
        {
            if(fileSystem == null)
                throw new ArgumentNullException("fileSystem");

            ValidateDirectoryPathInternal(fileSystem, destinationDirectoryPath, "destinationDirectoryPath");

            ValidateDirectoryPathInternal(fileSystem, sourceDirectoryPath, "sourceDirectoryPath");

            this._destinationDirectory = fileSystem.DirectoryInfo.FromDirectoryName(destinationDirectoryPath);
            this._fileSystem = fileSystem;
            this._sourceDirectory = fileSystem.DirectoryInfo.FromDirectoryName(sourceDirectoryPath);
        }

        #endregion

        #region Properties

        protected internal virtual DirectoryInfoBase DestinationDirectory
        {
            get { return this._destinationDirectory; }
        }

        protected internal virtual IList<DirectoryInfoBase> DirectoriesCreatedOnTransfer
        {
            get { return this._directoriesCreatedOnTransfer; }
        }

        protected internal virtual IFileSystem FileSystem
        {
            get { return this._fileSystem; }
        }

        protected internal virtual IList<FileInfoBase> FilesCreatedOnTransfer
        {
            get { return this._filesCreatedOnTransfer; }
        }

        protected internal virtual bool IncludeBinaries
        {
            get { return this._includeBinaries; }
            set { this._includeBinaries = value; }
        }

        protected internal virtual bool IncludePdbFiles { get; set; }

        protected internal virtual IList<IFileTransferItem> Items
        {
            get { return this._items; }
        }

        protected internal virtual StringComparison NameComparison
        {
            get { return _nameComparison; }
        }

        protected internal virtual DirectoryInfoBase SourceDirectory
        {
            get { return this._sourceDirectory; }
        }

        #endregion

        #region Methods

        public virtual void AddFile(string sourceFilePath)
        {
            this.AddFile(sourceFilePath, null, false);
        }

        public virtual void AddFile(string sourceFilePath, string destinationPath)
        {
            this.AddFile(sourceFilePath, destinationPath, true);
        }

        protected internal virtual void AddFile(string sourceFilePath, string destinationPath, bool validateDestinationPath)
        {
            this.ValidateSourceFilePath(sourceFilePath);

            if(validateDestinationPath)
            {
                if(destinationPath == null)
                    throw new ArgumentNullException("destinationPath");

                if(!this.IsDestinationPath(destinationPath))
                    throw new IOException(string.Format(CultureInfo.InvariantCulture, "The path \"{0}\" is not a valid destination-path.", destinationPath));
            }
            else
            {
                if(destinationPath != null)
                    throw new InvalidOperationException("If the destination-path should not be validated it must be null.");
            }

            FileInfoBase source = this.FileSystem.FileInfo.FromFileName(sourceFilePath);

            if(destinationPath == null)
            {
                // ReSharper disable ConvertIfStatementToConditionalTernaryExpression
                if(this.IsSourcePath(source.FullName))
                    destinationPath = Path.Combine(this.DestinationDirectory.FullName, source.FullName.Substring(this.SourceDirectory.FullName.Length));
                else
                    destinationPath = Path.Combine(this.DestinationDirectory.FullName, source.Name);
                // ReSharper restore ConvertIfStatementToConditionalTernaryExpression
            }

            if(this.IsRelativePath(destinationPath))
                destinationPath = Path.Combine(this.DestinationDirectory.FullName, destinationPath);

            FileInfoBase destination = this.FileSystem.FileInfo.FromFileName(destinationPath);

            if(string.IsNullOrEmpty(destination.Name))
                destination = this.FileSystem.FileInfo.FromFileName(Path.Combine(destinationPath, source.Name));

            IFileTransferItem fileTransferItem = new FileTransferItem
                {
                    Destination = destination,
                    Source = source
                };

            if(this.Items.Contains(fileTransferItem))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "An item with source \"{0}\" and destination \"{1}\" has already been added.", source.FullName, destination.FullName));

            this.Items.Add(fileTransferItem);
        }

        protected internal virtual IEnumerable<FileInfoBase> GetSourceBinaries()
        {
            return this.GetSourceBinaries(this.IncludePdbFiles);
        }

        protected internal virtual IEnumerable<FileInfoBase> GetSourceBinaries(bool includePdbFiles)
        {
            return this.SourceDirectory.GetFiles("*.dll", SearchOption.AllDirectories).Concat(includePdbFiles ? this.SourceDirectory.GetFiles("*.pdb", SearchOption.AllDirectories) : new FileInfoBase[0]).Where(fileInfo => !fileInfo.FullName.StartsWith(Path.Combine(this.SourceDirectory.FullName, "bin"), StringComparison.OrdinalIgnoreCase)).OrderBy(fileInfo => fileInfo.FullName);
        }

        protected internal virtual bool IsDestinationPath(string path)
        {
            if(path == null)
                throw new ArgumentNullException("path");

            if(this.IsRelativePath(path))
                return true;

            return path.StartsWith(this.DestinationDirectory.FullName, this.NameComparison);
        }

        protected internal virtual bool IsRelativePath(string path)
        {
            if(path == null)
                throw new ArgumentNullException("path");

            return !Path.IsPathRooted(path);
        }

        protected internal virtual bool IsSourcePath(string path)
        {
            if(path == null)
                throw new ArgumentNullException("path");

            return path.StartsWith(this.SourceDirectory.FullName, this.NameComparison);
        }

        public virtual void Reset()
        {
            for(int i = this.DirectoriesCreatedOnTransfer.Count - 1; i >= 0; i--)
            {
                DirectoryInfoBase directory = this.DirectoriesCreatedOnTransfer[i];

                if(this.FileSystem.Directory.Exists(directory.FullName))
                    this.FileSystem.Directory.Delete(directory.FullName, true);

                this.DirectoriesCreatedOnTransfer.RemoveAt(i);
            }

            for(int i = this.FilesCreatedOnTransfer.Count - 1; i >= 0; i--)
            {
                FileInfoBase file = this.FilesCreatedOnTransfer[i];

                if(this.FileSystem.File.Exists(file.FullName))
                    this.FileSystem.File.Delete(file.FullName);

                this.FilesCreatedOnTransfer.RemoveAt(i);
            }
        }

        public virtual void Transfer()
        {
            if(this.IncludeBinaries)
            {
                foreach(FileInfoBase source in this.GetSourceBinaries())
                {
                    string destinationPath = Path.Combine(Path.Combine(this.DestinationDirectory.FullName, "bin"), source.FullName.Substring(this.SourceDirectory.FullName.Length).TrimStart(@"\".ToCharArray()));
                    FileInfoBase destination = this.FileSystem.FileInfo.FromFileName(destinationPath);
                    IFileTransferItem fileTransferItem = new FileTransferItem {Source = source, Destination = destination};

                    if(!this.Items.Contains(fileTransferItem))
                        this.Items.Add(fileTransferItem);
                }
            }

            IEnumerable<IFileTransferItem> fileTransferItems = this.Items.OrderBy(item => item.Destination.FullName).ToArray();

            foreach(DirectoryInfoBase destinationDirectory in fileTransferItems.Select(item => item.Destination.Directory))
            {
                if(this.FileSystem.Directory.Exists(destinationDirectory.FullName))
                    continue;

                this.DirectoriesCreatedOnTransfer.Add(destinationDirectory);
                this.FileSystem.Directory.CreateDirectory(destinationDirectory.FullName);
            }

            foreach(IFileTransferItem fileTransferItem in fileTransferItems)
            {
                string destinationPath = fileTransferItem.Destination.FullName;
                string sourcePath = fileTransferItem.Source.FullName;

                if(!this.FileSystem.File.Exists(destinationPath))
                    this.FilesCreatedOnTransfer.Add(fileTransferItem.Destination);

                if(!this.FileSystem.File.Exists(destinationPath) || this.FileSystem.File.GetCreationTimeUtc(destinationPath) < this.FileSystem.File.GetCreationTimeUtc(sourcePath))
                    this.FileSystem.File.Copy(sourcePath, destinationPath, true);
            }
        }

        protected internal virtual void ValidateDirectoryPath(string directoryPath, string parameterName)
        {
            ValidateDirectoryPathInternal(this.FileSystem, directoryPath, parameterName);
        }

        protected internal static void ValidateDirectoryPathInternal(IFileSystem fileSystem, string directoryPath, string parameterName)
        {
            if(fileSystem == null)
                throw new ArgumentNullException("fileSystem");

            if(directoryPath == null)
                throw new ArgumentNullException(parameterName ?? string.Empty);

            if(!fileSystem.Directory.Exists(directoryPath))
                throw new IOException(string.Format(CultureInfo.InvariantCulture, _directoryDoesNotExistExceptionMessageFormat, directoryPath));
        }

        protected internal virtual void ValidateFilePath(string filePath, string parameterName)
        {
            if(filePath == null)
                throw new ArgumentNullException(parameterName ?? string.Empty);

            if(!this.FileSystem.File.Exists(filePath))
                throw new IOException(string.Format(CultureInfo.InvariantCulture, _fileDoesNotExistExceptionMessageFormat, filePath));
        }

        protected internal virtual void ValidateSourceFilePath(string sourceFilePath)
        {
            this.ValidateFilePath(sourceFilePath, "sourceFilePath");
        }

        #endregion
    }
}