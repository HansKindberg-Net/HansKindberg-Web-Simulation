namespace HansKindberg.Web.Simulation.Applications.Logic.IoC
{
    public static class DependencyResolver
    {
        #region Fields

        private static IDependencyResolver _instance;

        #endregion

        #region Properties

        public static IDependencyResolver Instance
        {
            get { return _instance ?? (_instance = new DefaultDependencyResolver()); }
            set { _instance = value; }
        }

        #endregion
    }
}