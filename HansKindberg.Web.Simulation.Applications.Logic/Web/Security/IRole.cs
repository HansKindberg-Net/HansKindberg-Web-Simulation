using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.Web.Simulation.Applications.Logic.Web.Security
{
    public interface IRole : IPrincipal
    {
        #region Properties

        IEnumerable<Guid> Users { get; }

        #endregion

        #region Methods

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        void AddUser(Guid userGuid);

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        bool RemoveUser(Guid userGuid);

        #endregion
    }
}