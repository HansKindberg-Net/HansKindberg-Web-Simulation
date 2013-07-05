using System;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class VirtualApplicationHostProxy : VirtualApplicationHostProxy<VirtualApplicationHost, RequestResult>
    {
        #region Constructors

        public VirtualApplicationHostProxy(string physicalDirectoryPath, string virtualPath, IApplicationHostFactory applicationHostFactory, IFileTransfer fileTransfer, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime, SimulatedVirtualPathProvider virtualPathProvider) : base(physicalDirectoryPath, virtualPath, applicationHostFactory, fileTransfer, httpApplicationManager, httpRuntime, virtualPathProvider) {}

        #endregion
    }

    public abstract class VirtualApplicationHostProxy<TApplicationHost, TRequestResult> : ApplicationHostProxy<TApplicationHost, TRequestResult> where TApplicationHost : VirtualApplicationHost<TRequestResult> where TRequestResult : RequestResult, new()
    {
        #region Fields

        private readonly SimulatedVirtualPathProvider _virtualPathProvider;

        #endregion

        #region Constructors

        protected VirtualApplicationHostProxy(string physicalDirectoryPath, string virtualPath, IApplicationHostFactory applicationHostFactory, IFileTransfer fileTransfer, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime, SimulatedVirtualPathProvider virtualPathProvider) : base(physicalDirectoryPath, virtualPath, applicationHostFactory, fileTransfer, httpApplicationManager, httpRuntime)
        {
            if(virtualPathProvider == null)
                throw new ArgumentNullException("virtualPathProvider");

            this._virtualPathProvider = virtualPathProvider;
        }

        #endregion

        #region Properties

        public virtual SimulatedVirtualPathProvider VirtualPathProvider
        {
            get { return this.ApplicationHost.VirtualPathProvider; }
        }

        #endregion

        #region Methods

        protected internal override void Initialize(TApplicationHost applicationHost, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime)
        {
            if(applicationHost == null)
                throw new ArgumentNullException("applicationHost");

            applicationHost.VirtualPathProvider = this._virtualPathProvider;

            base.Initialize(applicationHost, httpApplicationManager, httpRuntime);
        }

        #endregion
    }
}