using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using HansKindberg.Web.Mvc.Simulation.Application.Business.Configuration;

namespace HansKindberg.Web.Mvc.Simulation.Application
{
    public class Global : System.Web.HttpApplication
    {
        #region Methods

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void Application_Start()
        {
            Bootstrapper.Bootstrap();

            AreaRegistration.RegisterAllAreas();

            WebApiConfiguration.Register(GlobalConfiguration.Configuration);
            FilterConfiguration.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfiguration.RegisterRoutes(RouteTable.Routes);
        }

        #endregion
    }
}