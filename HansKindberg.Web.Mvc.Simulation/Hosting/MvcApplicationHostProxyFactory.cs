using System;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using HansKindberg.Web.Simulation;
using HansKindberg.Web.Simulation.Hosting;

namespace HansKindberg.Web.Mvc.Simulation.Hosting
{
    public class MvcApplicationHostProxyFactory : ApplicationHostProxyFactoryBase, IMvcApplicationHostProxyFactory
    {
        #region Constructors

        public MvcApplicationHostProxyFactory() {}
        public MvcApplicationHostProxyFactory(IFileSystem fileSystem, IApplicationHostFactory applicationHostFactory, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime) : base(fileSystem, applicationHostFactory, httpApplicationManager, httpRuntime) {}

        #endregion

        #region Methods

        public virtual MvcApplicationHostProxy Create(string physicalDirectoryPath)
        {
            return this.Create(physicalDirectoryPath, this.DefaultVirtualPath);
        }

        public virtual MvcApplicationHostProxy Create(string physicalDirectoryPath, string virtualPath)
        {
            if(!this.FileSystem.Directory.Exists(physicalDirectoryPath))
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "The directory \"{0}\" does not exist.", physicalDirectoryPath));

            return new MvcApplicationHostProxy(physicalDirectoryPath, virtualPath, this.ApplicationHostFactory, new FileTransfer(this.FileSystem, physicalDirectoryPath, AppDomain.CurrentDomain.BaseDirectory), this.HttpApplicationManager, this.HttpRuntime);
        }

        #endregion
    }
}