using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace HansKindberg.Web.Simulation.Applications.Logic.IoC.StructureMap
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        #region Fields

        private readonly IContainer _container;
        private static IDependencyResolver _instance;

        #endregion

        #region Constructors

        public StructureMapDependencyResolver(IContainer container)
        {
            if(container == null)
                throw new ArgumentNullException("container");

            this._container = container;
        }

        #endregion

        #region Properties

        protected internal virtual IContainer Container
        {
            get { return this._container; }
        }

        public static IDependencyResolver Instance
        {
            get { return _instance ?? (_instance = new StructureMapDependencyResolver(ObjectFactory.Container)); }
            set { _instance = value; }
        }

        #endregion

        #region Methods

        protected internal virtual object GetConcreteService(Type serviceType)
        {
            try
            {
                // Can't use TryGetInstance here because it won’t create concrete types
                return this.Container.GetInstance(serviceType);
            }
            catch(StructureMapException)
            {
                return null;
            }
        }

        protected internal virtual object GetInterfaceService(Type serviceType)
        {
            return this.Container.TryGetInstance(serviceType);
        }

        public virtual object GetService(Type serviceType)
        {
            if(serviceType == null)
                throw new ArgumentNullException("serviceType");

            if(serviceType.IsInterface || serviceType.IsAbstract)
                return this.GetInterfaceService(serviceType);

            return this.GetConcreteService(serviceType);
        }

        public virtual T GetService<T>()
        {
            return (T) this.GetService(typeof(T));
        }

        public virtual IEnumerable<object> GetServices(Type serviceType)
        {
            return this.Container.GetAllInstances(serviceType).Cast<object>();
        }

        #endregion
    }
}