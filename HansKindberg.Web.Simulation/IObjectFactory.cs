using System;

namespace HansKindberg.Web.Simulation
{
    public interface IObjectFactory
    {
        #region Methods

        T CreateInstance<T>();
        object CreateInstance(Type type);

        #endregion
    }
}