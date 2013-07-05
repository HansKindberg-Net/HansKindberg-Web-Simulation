using System;
using System.Collections.Generic;
using System.Linq;
using HansKindberg.Web.Simulation.Applications.Logic.IoC;
using HansKindberg.Web.Simulation.Applications.Logic.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Fakes.Web.Security
{
    public class FakedRoleProvider : System.Web.Security.RoleProvider
    {
        #region Fields

        private readonly IPrincipalRepository _principalRepository;

        #endregion

        #region Constructors

        public FakedRoleProvider() : this(DependencyResolver.Instance.GetService<IPrincipalRepository>()) {}

        public FakedRoleProvider(IPrincipalRepository principalRepository)
        {
            if(principalRepository == null)
                throw new ArgumentNullException("principalRepository");

            this._principalRepository = principalRepository;
        }

        #endregion

        #region Properties

        public override string ApplicationName { get; set; }

        protected internal virtual IPrincipalRepository PrincipalRepository
        {
            get { return this._principalRepository; }
        }

        #endregion

        #region Methods

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return this.PrincipalRepository.Roles.OrderBy(role => role.Name).Select(role => role.Name).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            IEnumerable<IRole> rolesForUser = new IRole[0];

            IUser user = this.PrincipalRepository.GetUserByName(username);

            if(user != null)
                rolesForUser = this.PrincipalRepository.GetRoles(user.Guid);

            return rolesForUser.Select(role => role.Name).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            IRole role = this.PrincipalRepository.GetRoleByName(roleName);

            return this.PrincipalRepository.Users.Where(user => role != null && role.Users.Contains(user.Guid)).OrderBy(user => user.Name).Select(user => user.Name).ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            IRole role = this.PrincipalRepository.GetRoleByName(roleName);
            IUser user = this.PrincipalRepository.GetUserByName(username);

            return role != null && user != null && role.Users.Contains(user.Guid);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return !string.IsNullOrEmpty(roleName) && this.GetAllRoles().Contains(roleName);
        }

        #endregion
    }
}