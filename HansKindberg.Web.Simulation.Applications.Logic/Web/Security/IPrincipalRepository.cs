using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HansKindberg.Web.Simulation.Applications.Logic.Net.Mail;

namespace HansKindberg.Web.Simulation.Applications.Logic.Web.Security
{
    public interface IPrincipalRepository
    {
        #region Properties

        IEmailValidator EmailValidator { get; }

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        IEnumerable<IRole> Roles { get; }

        IEnumerable<IUser> Users { get; }

        #endregion

        #region Methods

        void AddRole(IRole role);
        void AddUser(IUser user);

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        IRole GetRoleByGuid(Guid guid);

        IRole GetRoleByName(string name);

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        IEnumerable<IRole> GetRoles(Guid userGuid);

        IUser GetUserByEmail(string emailAddress);

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        IUser GetUserByGuid(Guid guid);

        IUser GetUserByName(string name);

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        bool RemoveRole(Guid roleGuid);

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        bool RemoveUser(Guid userGuid);

        #endregion
    }
}