using System;
using System.Collections.Generic;

namespace HansKindberg.Web.Simulation.Applications.Logic.IoC
{
    public interface IDependencyResolver
    {
        #region Methods

        object GetService(Type serviceType);
        T GetService<T>();
        IEnumerable<object> GetServices(Type serviceType);

        #endregion
    }
}