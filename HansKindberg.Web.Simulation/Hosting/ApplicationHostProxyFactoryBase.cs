using System;
using System.IO.Abstractions;

namespace HansKindberg.Web.Simulation.Hosting
{
    public abstract class ApplicationHostProxyFactoryBase
    {
        #region Fields

        private readonly IApplicationHostFactory _applicationHostFactory;
        private const string _defaultVirtualPath = "/";
        private readonly IFileSystem _fileSystem;
        private readonly IHttpApplicationManager _httpApplicationManager;
        private readonly IHttpRuntime _httpRuntime;

        #endregion

        #region Constructors

        protected ApplicationHostProxyFactoryBase() : this(new FileSystem(), new ApplicationHostFactory(), new HttpApplicationManager(), new HttpRuntimeWrapper()) {}

        protected ApplicationHostProxyFactoryBase(IFileSystem fileSystem, IApplicationHostFactory applicationHostFactory, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime)
        {
            if(fileSystem == null)
                throw new ArgumentNullException("fileSystem");

            if(applicationHostFactory == null)
                throw new ArgumentNullException("applicationHostFactory");

            if(httpApplicationManager == null)
                throw new ArgumentNullException("httpApplicationManager");

            if(httpRuntime == null)
                throw new ArgumentNullException("httpRuntime");

            this._applicationHostFactory = applicationHostFactory;
            this._fileSystem = fileSystem;
            this._httpApplicationManager = httpApplicationManager;
            this._httpRuntime = httpRuntime;
        }

        #endregion

        #region Properties

        protected internal virtual IApplicationHostFactory ApplicationHostFactory
        {
            get { return this._applicationHostFactory; }
        }

        protected internal virtual string DefaultVirtualPath
        {
            get { return _defaultVirtualPath; }
        }

        protected internal virtual IFileSystem FileSystem
        {
            get { return this._fileSystem; }
        }

        protected internal virtual IHttpApplicationManager HttpApplicationManager
        {
            get { return this._httpApplicationManager; }
        }

        protected internal virtual IHttpRuntime HttpRuntime
        {
            get { return this._httpRuntime; }
        }

        #endregion
    }
}