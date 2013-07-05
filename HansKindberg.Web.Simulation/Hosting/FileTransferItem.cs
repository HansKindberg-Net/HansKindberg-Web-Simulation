using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using HansKindberg.Web.Simulation.IO;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class FileTransferItem : IFileTransferItem
    {
        #region Fields

        private readonly IEqualityComparer<FileInfoBase> _fileInfoEqualityComparer = new FileInfoEqualityComparer(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region Properties

        public virtual FileInfoBase Destination { get; set; }

        protected internal virtual IEqualityComparer<FileInfoBase> FileInfoEqualityComparer
        {
            get { return this._fileInfoEqualityComparer; }
        }

        public virtual FileInfoBase Source { get; set; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(this, obj))
                return true;

            IFileTransferItem fileTransferItem = obj as IFileTransferItem;

            if(fileTransferItem == null)
                return false;

            return this.FileInfoEqualityComparer.Equals(this.Destination, fileTransferItem.Destination) && this.FileInfoEqualityComparer.Equals(this.Source, fileTransferItem.Source);
        }

        public override int GetHashCode()
        {
            return this.FileInfoEqualityComparer.GetHashCode(this.Destination) ^ this.FileInfoEqualityComparer.GetHashCode(this.Source);
        }

        #endregion
    }
}