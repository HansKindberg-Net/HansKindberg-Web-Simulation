using System;

namespace HansKindberg.Web.Simulation.Applications.Logic.Web.Security
{
    public interface IPrincipal
    {
        #region Properties

        Guid Guid { get; }
        string Name { get; }

        #endregion
    }
}