using System;
using System.Diagnostics.CodeAnalysis;
using HansKindberg.Web.Simulation.Serialization;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class ApplicationHostProxy : ApplicationHostProxy<ApplicationHost, RequestResult>
    {
        #region Constructors

        public ApplicationHostProxy(string physicalDirectoryPath, string virtualPath, IApplicationHostFactory applicationHostFactory, IFileTransfer fileTransfer, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime) : base(physicalDirectoryPath, virtualPath, applicationHostFactory, fileTransfer, httpApplicationManager, httpRuntime) {}

        #endregion
    }

    public abstract class ApplicationHostProxy<TApplicationHost, TRequestResult> : IDisposable where TApplicationHost : ApplicationHost<TRequestResult> where TRequestResult : RequestResult, new()
    {
        #region Fields

        private TApplicationHost _applicationHost;
        private readonly IApplicationHostFactory _applicationHostFactory;
        private readonly IFileTransfer _fileTransfer;
        private readonly IHttpApplicationManager _httpApplicationManager;
        private readonly IHttpRuntime _httpRuntime;
        private readonly string _physicalDirectoryPath;
        private readonly string _virtualPath;

        #endregion

        #region Constructors

        protected ApplicationHostProxy(string physicalDirectoryPath, string virtualPath, IApplicationHostFactory applicationHostFactory, IFileTransfer fileTransfer, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime)
        {
            if(applicationHostFactory == null)
                throw new ArgumentNullException("applicationHostFactory");

            if(fileTransfer == null)
                throw new ArgumentNullException("fileTransfer");

            if(httpApplicationManager == null)
                throw new ArgumentNullException("httpApplicationManager");

            if(httpRuntime == null)
                throw new ArgumentNullException("httpRuntime");

            this._applicationHostFactory = applicationHostFactory;
            this._fileTransfer = fileTransfer;
            this._httpApplicationManager = httpApplicationManager;
            this._httpRuntime = httpRuntime;
            this._physicalDirectoryPath = physicalDirectoryPath;
            this._virtualPath = virtualPath;
        }

        #endregion

        #region Events

        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        public event Action<HttpApplicationEvent> AnyApplicationEvent
        {
            add { this.ApplicationHost.AnyApplicationEvent += value; }
            remove { this.ApplicationHost.AnyApplicationEvent -= value; }
        }

        #endregion

        #region Properties

        protected internal virtual TApplicationHost ApplicationHost
        {
            get
            {
                if(this._applicationHost == null)
                {
                    this.FileTransfer.Transfer();

                    this._applicationHost = this.ApplicationHostFactory.Create<TApplicationHost, TRequestResult>(this.VirtualPath, this.PhysicalDirectoryPath);

                    this.Initialize(this._applicationHost, this.HttpApplicationManager, this.HttpRuntime);
                }

                return this._applicationHost;
            }
        }

        protected internal virtual IApplicationHostFactory ApplicationHostFactory
        {
            get { return this._applicationHostFactory; }
        }

        protected internal virtual IFileTransfer FileTransfer
        {
            get { return this._fileTransfer; }
        }

        protected internal virtual IHttpApplicationManager HttpApplicationManager
        {
            get { return this._httpApplicationManager; }
        }

        protected internal virtual IHttpRuntime HttpRuntime
        {
            get { return this._httpRuntime; }
        }

        protected internal virtual string PhysicalDirectoryPath
        {
            get { return this._physicalDirectoryPath; }
        }

        protected internal virtual string VirtualPath
        {
            get { return this._virtualPath; }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposing)
                return;

            if(this._applicationHost != null)
                this._applicationHost.Dispose();

            this.FileTransfer.Reset();
        }

        protected internal virtual void Initialize(TApplicationHost applicationHost, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime)
        {
            if(applicationHost == null)
                throw new ArgumentNullException("applicationHost");

            if(httpApplicationManager == null)
                throw new ArgumentNullException("httpApplicationManager");

            if(httpRuntime == null)
                throw new ArgumentNullException("httpRuntime");

            applicationHost.InitializeApplication(httpApplicationManager, httpRuntime);

            applicationHost.Run(() => RequestResult.Instance<TRequestResult>().Clear());
        }

        public virtual void Run(Action code)
        {
            this.ApplicationHost.Run(code);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public virtual void Run(Action<BrowsingSession<TRequestResult>> browsingSession)
        {
            var serializableDelegate = new SerializableDelegate<Action<BrowsingSession<TRequestResult>>>(browsingSession);
            this.ApplicationHost.Run(serializableDelegate);
        }

        #endregion
    }
}