using System;
using System.IO.Abstractions;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class VirtualApplicationHostProxyFactory : ApplicationHostProxyFactoryBase, IVirtualApplicationHostProxyFactory
    {
        #region Fields

        private static readonly string _defaultPhysicalDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

        #endregion

        #region Constructors

        public VirtualApplicationHostProxyFactory() {}
        public VirtualApplicationHostProxyFactory(IFileSystem fileSystem, IApplicationHostFactory applicationHostFactory, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime) : base(fileSystem, applicationHostFactory, httpApplicationManager, httpRuntime) {}

        #endregion

        #region Properties

        protected internal virtual string DefaultPhysicalDirectoryPath
        {
            get { return _defaultPhysicalDirectoryPath; }
        }

        #endregion

        #region Methods

        public virtual VirtualApplicationHostProxy Create()
        {
            return this.Create(this.DefaultVirtualPath);
        }

        public virtual VirtualApplicationHostProxy Create(string virtualPath)
        {
            return this.Create(virtualPath, this.CreateFileTransfer());
        }

        public virtual VirtualApplicationHostProxy Create(IFileTransfer fileTransfer)
        {
            return this.Create(this.DefaultVirtualPath, fileTransfer);
        }

        public virtual VirtualApplicationHostProxy Create(SimulatedVirtualPathProvider virtualPathProvider)
        {
            return this.Create(this.DefaultVirtualPath, this.CreateFileTransfer(), virtualPathProvider);
        }

        public virtual VirtualApplicationHostProxy Create(string virtualPath, IFileTransfer fileTransfer)
        {
            return this.Create(virtualPath, fileTransfer, new SimulatedVirtualPathProvider());
        }

        public virtual VirtualApplicationHostProxy Create(string virtualPath, SimulatedVirtualPathProvider virtualPathProvider)
        {
            return this.Create(virtualPath, this.CreateFileTransfer(), virtualPathProvider);
        }

        public virtual VirtualApplicationHostProxy Create(IFileTransfer fileTransfer, SimulatedVirtualPathProvider virtualPathProvider)
        {
            return this.Create(this.DefaultVirtualPath, fileTransfer, virtualPathProvider);
        }

        public virtual VirtualApplicationHostProxy Create(string virtualPath, IFileTransfer fileTransfer, SimulatedVirtualPathProvider virtualPathProvider)
        {
            return new VirtualApplicationHostProxy(this.DefaultPhysicalDirectoryPath, virtualPath, this.ApplicationHostFactory, fileTransfer, this.HttpApplicationManager, this.HttpRuntime, virtualPathProvider);
        }

        protected internal virtual IFileTransfer CreateFileTransfer()
        {
            return new FileTransfer(this.FileSystem, this.DefaultPhysicalDirectoryPath, this.DefaultPhysicalDirectoryPath);
        }

        #endregion
    }
}