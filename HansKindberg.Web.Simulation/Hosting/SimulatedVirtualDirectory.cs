using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class SimulatedVirtualDirectory : VirtualDirectory
    {
        #region Fields

        private readonly IEnumerable<VirtualFileBase> _virtualFiles;
        private const StringComparison _virtualPathComparison = StringComparison.OrdinalIgnoreCase;

        #endregion

        #region Constructors

        public SimulatedVirtualDirectory(string virtualPath, IEnumerable<VirtualFileBase> virtualFiles) : base(virtualPath)
        {
            if(virtualFiles == null)
                throw new ArgumentNullException("virtualFiles");

            this._virtualFiles = virtualFiles;
        }

        #endregion

        #region Properties

        public override IEnumerable Children
        {
            get { return this.GetChildren(); }
        }

        public override IEnumerable Directories
        {
            get { return this.GetChildren().Where(file => file.IsDirectory); }
        }

        public override IEnumerable Files
        {
            get { return this.GetChildren().Where(file => !file.IsDirectory); }
        }

        protected internal virtual IEnumerable<VirtualFileBase> VirtualFiles
        {
            get { return this._virtualFiles; }
        }

        protected internal virtual StringComparison VirtualPathComparison
        {
            get { return _virtualPathComparison; }
        }

        #endregion

        #region Methods

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected internal virtual IEnumerable<VirtualFileBase> GetChildren()
        {
            return this.VirtualFiles.Where(this.IsChild).OrderBy(file => file.IsDirectory).ThenBy(file => file.Name).ToArray();
        }

        protected internal virtual bool IsChild(VirtualFileBase potentialChild)
        {
            if(potentialChild == null)
                throw new ArgumentNullException("potentialChild");

            return this.VirtualPath.Equals(VirtualPathUtility.GetDirectory(potentialChild.VirtualPath), this.VirtualPathComparison);
        }

        #endregion
    }
}