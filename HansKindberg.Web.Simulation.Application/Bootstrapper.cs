using HansKindberg.Web.Simulation.Application.Business.Web.Mvp.IoC.StructureMap.Binder;
using HansKindberg.Web.Simulation.Applications.Logic.IoC;
using HansKindberg.Web.Simulation.Applications.Logic.IoC.StructureMap;
using StructureMap;
using WebFormsMvp.Binder;

namespace HansKindberg.Web.Simulation.Application
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
            DependencyResolver.Instance = new StructureMapDependencyResolver(container);
            PresenterBinder.Factory = new StructureMapPresenterFactory(container);
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