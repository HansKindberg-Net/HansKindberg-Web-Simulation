using System;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class ApplicationHostProxyFactory : ApplicationHostProxyFactoryBase, IApplicationHostProxyFactory
    {
        #region Constructors

        public ApplicationHostProxyFactory() {}
        public ApplicationHostProxyFactory(IFileSystem fileSystem, IApplicationHostFactory applicationHostFactory, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime) : base(fileSystem, applicationHostFactory, httpApplicationManager, httpRuntime) {}

        #endregion

        #region Methods

        public virtual ApplicationHostProxy Create(string physicalDirectoryPath)
        {
            return this.Create(physicalDirectoryPath, this.DefaultVirtualPath);
        }

        public virtual ApplicationHostProxy Create(string physicalDirectoryPath, string virtualPath)
        {
            if(!this.FileSystem.Directory.Exists(physicalDirectoryPath))
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "The directory \"{0}\" does not exist.", physicalDirectoryPath));

            return new ApplicationHostProxy(physicalDirectoryPath, virtualPath, this.ApplicationHostFactory, new FileTransfer(this.FileSystem, physicalDirectoryPath, AppDomain.CurrentDomain.BaseDirectory), this.HttpApplicationManager, this.HttpRuntime);
        }

        #endregion
    }
}