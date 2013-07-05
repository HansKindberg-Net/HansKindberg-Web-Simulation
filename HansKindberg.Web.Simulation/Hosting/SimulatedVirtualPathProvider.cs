using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Caching;
using System.Web.Hosting;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class SimulatedVirtualPathProvider : VirtualPathProvider
    {
        #region Fields

        private readonly VirtualFileCollection _virtualFiles = new VirtualFileCollection();
        private const StringComparison _virtualPathComparison = StringComparison.OrdinalIgnoreCase;

        #endregion

        #region Constructors

        public SimulatedVirtualPathProvider()
        {
            this._virtualFiles.Add(new SimulatedVirtualDirectory("/", this._virtualFiles));
        }

        #endregion

        #region Properties

        public virtual VirtualFileCollection VirtualFiles
        {
            get { return this._virtualFiles; }
        }

        protected internal virtual StringComparison VirtualPathComparison
        {
            get { return _virtualPathComparison; }
        }

        #endregion

        #region Methods

        public override bool DirectoryExists(string virtualDir)
        {
            return this.GetDirectory(virtualDir) != null;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public override bool FileExists(string virtualPath)
        {
            // Do not really know why I put this check here, but I leave it for the time being.
            if(virtualPath != null && virtualPath.ToLowerInvariant().Contains("web.config"))
                throw new InvalidOperationException("Web.config, not sure why we throw this exception.");

            return this.GetFile(virtualPath) != null;
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return null;
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            return this.VirtualFiles.Where(virtualFile => virtualFile.VirtualPath.Equals(virtualDir, this.VirtualPathComparison) && virtualFile.IsDirectory).Cast<VirtualDirectory>().FirstOrDefault();
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            return this.VirtualFiles.Where(virtualFile => virtualFile.VirtualPath.Equals(virtualPath, this.VirtualPathComparison) && !virtualFile.IsDirectory).Cast<VirtualFile>().FirstOrDefault();
        }

        #endregion
    }
}