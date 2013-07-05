using HansKindberg.Web.Mvc.Simulation.Application.Business.Web.Mvc;
using HansKindberg.Web.Simulation.Applications.Logic.IoC;
using StructureMap;

namespace HansKindberg.Web.Mvc.Simulation.Application
{
    public class Bootstrapper : IBootstrapper
    {
        #region Fields

        private static bool _hasStarted;

        #endregion

        #region Methods

        public static void Bootstrap()
        {
            new Bootstrapper().BootstrapStructureMap();
            IContainer container = ObjectFactory.Container;
            MvcDependencyResolver mvcDependencyResolver = new MvcDependencyResolver(container);
            DependencyResolver.Instance = mvcDependencyResolver;
            System.Web.Mvc.DependencyResolver.SetResolver(mvcDependencyResolver);
        }

        public void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(initializer => { initializer.PullConfigurationFromAppConfig = true; });
        }

        public static void Restart()
        {
            if(_hasStarted)
            {
                ObjectFactory.ResetDefaults();
            }
            else
            {
                Bootstrap();
                _hasStarted = true;
            }
        }

        #endregion
    }
}