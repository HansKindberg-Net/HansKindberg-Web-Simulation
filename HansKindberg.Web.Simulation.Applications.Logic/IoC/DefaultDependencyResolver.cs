using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HansKindberg.Web.Simulation.Applications.Logic.IoC
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        #region Methods

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public virtual object GetService(Type serviceType)
        {
            if(serviceType == null)
                throw new ArgumentNullException("serviceType");

            if(serviceType.IsInterface || serviceType.IsAbstract)
                return null;

            try
            {
                return Activator.CreateInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public virtual T GetService<T>()
        {
            return (T) this.GetService(typeof(T));
        }

        public virtual IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }

        #endregion
    }
}