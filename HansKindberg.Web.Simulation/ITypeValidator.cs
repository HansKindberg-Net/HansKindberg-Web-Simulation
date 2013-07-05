using System;

namespace HansKindberg.Web.Simulation
{
    public interface ITypeValidator
    {
        #region Methods

        void ValidateThatTheTypeIsADelegate(Type type);

        #endregion
    }
}