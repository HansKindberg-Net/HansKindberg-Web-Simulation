using System;
using System.Web.Http;

namespace HansKindberg.Web.Mvc.Simulation.Application.Business.Configuration
{
    public static class WebApiConfiguration
    {
        #region Methods

        public static void Register(HttpConfiguration configuration)
        {
            if(configuration == null)
                throw new ArgumentNullException("configuration");

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
        }

        #endregion
    }
}