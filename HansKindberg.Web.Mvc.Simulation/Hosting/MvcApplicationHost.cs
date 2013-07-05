using System.Web.Mvc;
using HansKindberg.Web.Simulation;
using HansKindberg.Web.Simulation.Hosting;

namespace HansKindberg.Web.Mvc.Simulation.Hosting
{
    public class MvcApplicationHost : MvcApplicationHost<RequestResult> {}

    public abstract class MvcApplicationHost<TRequestResult> : ApplicationHost<TRequestResult> where TRequestResult : RequestResult, new()
    {
        #region Methods

        public override void InitializeApplication(IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime)
        {
            base.InitializeApplication(httpApplicationManager, httpRuntime);

            FilterProviders.Providers.Add(new InterceptionFilterProvider());
        }

        #endregion
    }
}