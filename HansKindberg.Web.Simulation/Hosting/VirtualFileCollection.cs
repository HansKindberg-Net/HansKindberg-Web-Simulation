using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class VirtualFileCollection : ICollection<VirtualFileBase>
    {
        #region Fields

        private static readonly Encoding _defaultEncoding = Encoding.UTF8;
        private const string _defaultWebFormFileContentFormat = "<%@ Page Language=\"C#\" AutoEventWireup=\"false\" Inherits=\"System.Web.UI.Page\" %><html><head><title>{0}</title></head><body><h1>{1}</h1><p>{2}</p></body></html>";
        private const string _itemAlreadyExistsExceptionMessageFormat = "An item with the virtual path \"{0}\" already exists.";
        private readonly IList<VirtualFileBase> _list = new List<VirtualFileBase>();
        private const string _virtualPathCanNotBeEmptyExceptionMessage = "The virtual path can not be empty.";
        private const StringComparison _virtualPathComparison = StringComparison.OrdinalIgnoreCase;

        #endregion

        #region Properties

        public virtual int Count
        {
            get { return this.List.Count; }
        }

        protected internal virtual Encoding DefaultEncoding
        {
            get { return _defaultEncoding; }
        }

        protected internal virtual string DefaultWebFormFileContentFormat
        {
            get { return _defaultWebFormFileContentFormat; }
        }

        public virtual bool IsReadOnly
        {
            get { return this.List.IsReadOnly; }
        }

        protected internal virtual IList<VirtualFileBase> List
        {
            get { return this._list; }
        }

        protected internal virtual StringComparison VirtualPathComparison
        {
            get { return _virtualPathComparison; }
        }

        #endregion

        #region Methods

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The argument is validated in another method.")]
        public virtual void Add(VirtualFileBase item)
        {
            this.ValidateItem(item);

            this.AddAncestorsIfNecessary(item.VirtualPath);

            this.List.Add(item);
        }

        protected internal virtual void AddAncestorsIfNecessary(string virtualPath)
        {
            if(virtualPath == null)
                throw new ArgumentNullException("virtualPath");

            if(virtualPath.Length == 0)
                throw new ArgumentException("The virtual path can not be empty.", "virtualPath");

            string parentVirtualPath = VirtualPathUtility.GetDirectory(virtualPath);

            while(parentVirtualPath != null)
            {
                if(!this.Contains(parentVirtualPath))
                    this.List.Add(new SimulatedVirtualDirectory(parentVirtualPath, this));

                parentVirtualPath = VirtualPathUtility.GetDirectory(parentVirtualPath);
            }
        }

        public virtual void AddRange(IEnumerable<VirtualFileBase> collection)
        {
            if(collection == null)
                throw new ArgumentNullException("collection");

            IEnumerable<VirtualFileBase> collectionCopy = collection.ToArray();

            foreach(VirtualFileBase item in collectionCopy)
            {
                this.ValidateItem(item);
            }

            foreach(VirtualFileBase item in collectionCopy)
            {
                this.Add(item);
            }
        }

        public virtual void Clear()
        {
            this.List.Clear();
        }

        public virtual bool Contains(string virtualPath)
        {
            if(virtualPath == null)
                throw new ArgumentNullException("virtualPath");

            return this.List.Any(item => item.VirtualPath.Equals(virtualPath, this.VirtualPathComparison));
        }

        public virtual bool Contains(VirtualFileBase item)
        {
            return this.List.Contains(item);
        }

        public virtual void CopyTo(VirtualFileBase[] array, int arrayIndex)
        {
            this.List.CopyTo(array, arrayIndex);
        }

        public virtual void CreateAndAddDirectory(string virtualPath)
        {
            this.ValidateVirtualPath(virtualPath, true);

            this.Add(new SimulatedVirtualDirectory(virtualPath, this));
        }

        public virtual void CreateAndAddFile(string virtualPath, string fileContent)
        {
            this.CreateAndAddFile(virtualPath, fileContent, this.DefaultEncoding);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Can not handle it otherwise.")]
        public virtual void CreateAndAddFile(string virtualPath, string fileContent, Encoding encoding)
        {
            this.ValidateVirtualPath(virtualPath, false);

            if(fileContent == null)
                throw new ArgumentNullException("fileContent");

            if(encoding == null)
                throw new ArgumentNullException("encoding");

            this.Add(new SimulatedVirtualFile(virtualPath, fileContent, encoding));
        }

        public virtual void CreateWithDefaultWebFormContentAndAddFile(string virtualPath)
        {
            this.CreateWithDefaultWebFormContentAndAddFile(virtualPath, this.DefaultEncoding);
        }

        public virtual void CreateWithDefaultWebFormContentAndAddFile(string virtualPath, Encoding encoding)
        {
            if(virtualPath == null)
                throw new ArgumentNullException("virtualPath");

            IEnumerable<string> virtualPathParts = virtualPath.Trim().Trim("/".ToCharArray()).Split("/".ToCharArray());

            string name = string.Join(" - ", virtualPathParts.ToArray());

            string fileContent = string.Format(CultureInfo.CurrentCulture, this.DefaultWebFormFileContentFormat, new object[] {name, name, "The content for this page is generated."});

            this.CreateAndAddFile(virtualPath, fileContent, encoding);
        }

        public virtual IEnumerator<VirtualFileBase> GetEnumerator()
        {
            return this.List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public virtual int IndexOf(VirtualFileBase item)
        {
            return this.List.IndexOf(item);
        }

        public virtual bool Remove(VirtualFileBase item)
        {
            return this.List.Remove(item);
        }

        public virtual void RemoveAt(int index)
        {
            this.List.RemoveAt(index);
        }

        protected internal virtual void ValidateItem(VirtualFileBase item)
        {
            this.ValidateItem(item, "item");
        }

        protected internal virtual void ValidateItem(VirtualFileBase item, string parameterName)
        {
            if(parameterName == null)
                parameterName = string.Empty;

            if(item == null)
                throw new ArgumentNullException(parameterName, "The virtual file/directory can not be null.");

            this.ValidateVirtualPath(item.VirtualPath, item.IsDirectory, parameterName);
        }

        protected internal virtual void ValidateVirtualPath(string virtualPath, bool directory)
        {
            this.ValidateVirtualPath(virtualPath, directory, "virtualPath");
        }

        protected internal virtual void ValidateVirtualPath(string virtualPath, bool directory, string parameterName)
        {
            if(parameterName == null)
                parameterName = string.Empty;

            if(virtualPath == null)
                throw new ArgumentNullException(parameterName, "The virtual path can not be null.");

            if(virtualPath.Length == 0)
                throw new ArgumentException(_virtualPathCanNotBeEmptyExceptionMessage, parameterName);

            if(directory)
                virtualPath = VirtualPathUtility.AppendTrailingSlash(VirtualPathUtility.RemoveTrailingSlash(virtualPath));

            if(this.Contains(virtualPath))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, _itemAlreadyExistsExceptionMessageFormat, virtualPath), parameterName);
        }

        #endregion
    }
}