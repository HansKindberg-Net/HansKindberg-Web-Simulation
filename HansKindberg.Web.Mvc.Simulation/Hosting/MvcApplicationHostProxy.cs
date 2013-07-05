using HansKindberg.Web.Simulation;
using HansKindberg.Web.Simulation.Hosting;

namespace HansKindberg.Web.Mvc.Simulation.Hosting
{
    public class MvcApplicationHostProxy : MvcApplicationHostProxy<MvcApplicationHost, RequestResult>
    {
        #region Constructors

        public MvcApplicationHostProxy(string physicalDirectoryPath, string virtualPath, IApplicationHostFactory applicationHostFactory, IFileTransfer fileTransfer, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime) : base(physicalDirectoryPath, virtualPath, applicationHostFactory, fileTransfer, httpApplicationManager, httpRuntime) {}

        #endregion
    }

    public abstract class MvcApplicationHostProxy<TApplicationHost, TRequestResult> : ApplicationHostProxy<TApplicationHost, TRequestResult> where TApplicationHost : MvcApplicationHost<TRequestResult> where TRequestResult : RequestResult, new()
    {
        #region Constructors

        protected MvcApplicationHostProxy(string physicalDirectoryPath, string virtualPath, IApplicationHostFactory applicationHostFactory, IFileTransfer fileTransfer, IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime) : base(physicalDirectoryPath, virtualPath, applicationHostFactory, fileTransfer, httpApplicationManager, httpRuntime) {}

        #endregion
    }
}