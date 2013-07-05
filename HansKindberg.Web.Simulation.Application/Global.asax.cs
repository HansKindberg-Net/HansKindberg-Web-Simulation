using System;

namespace HansKindberg.Web.Simulation.Application
{
    public class Global : System.Web.HttpApplication
    {
        #region Methods

        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.Bootstrap();
        }

        #endregion
    }
}