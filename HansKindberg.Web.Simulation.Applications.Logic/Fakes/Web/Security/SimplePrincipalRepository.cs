using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using HansKindberg.Web.Simulation.Applications.Logic.Net.Mail;
using HansKindberg.Web.Simulation.Applications.Logic.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Fakes.Web.Security
{
    public class SimplePrincipalRepository : IPrincipalRepository
    {
        #region Fields

        private const string _emailDomain = "hanskindberg.web.mvc.simulation";
        private readonly IEmailValidator _emailValidator;
        private const StringComparison _roleNameComparison = StringComparison.OrdinalIgnoreCase;
        private readonly IList<IRole> _roles = new List<IRole>();
        private const StringComparison _userEmailComparison = StringComparison.OrdinalIgnoreCase;
        private const StringComparison _userNameComparison = StringComparison.OrdinalIgnoreCase;
        private readonly IList<IUser> _users = new List<IUser>();

        #endregion

        #region Constructors

        public SimplePrincipalRepository(IEmailValidator emailValidator)
        {
            if(emailValidator == null)
                throw new ArgumentNullException("emailValidator");

            this._emailValidator = emailValidator;

            SimpleUser admin = new SimpleUser("Admin", "password", "admin@" + _emailDomain);
            this._users.Add(admin);
            SimpleUser administrator = new SimpleUser("Administrator", "password", "administrator@" + _emailDomain);
            this._users.Add(administrator);
            SimpleUser editor = new SimpleUser("Editor", "password", "editor@" + _emailDomain);
            this._users.Add(editor);
            SimpleUser user = new SimpleUser("User", "password", "user@" + _emailDomain);
            this._users.Add(user);

            SimpleRole administrators = new SimpleRole("Administrators");
            administrators.AddUser(admin.Guid);
            administrators.AddUser(administrator.Guid);
            this._roles.Add(administrators);

            SimpleRole editors = new SimpleRole("Editors");
            editors.AddUser(admin.Guid);
            editors.AddUser(administrator.Guid);
            editors.AddUser(editor.Guid);
            this._roles.Add(editors);

            SimpleRole users = new SimpleRole("Users");
            users.AddUser(admin.Guid);
            users.AddUser(administrator.Guid);
            users.AddUser(editor.Guid);
            users.AddUser(user.Guid);
            this._roles.Add(users);

            SimpleRole webAdmins = new SimpleRole("WebAdmins");
            webAdmins.AddUser(admin.Guid);
            webAdmins.AddUser(administrator.Guid);
            this._roles.Add(webAdmins);

            SimpleRole webEditors = new SimpleRole("WebEditors");
            webEditors.AddUser(admin.Guid);
            webEditors.AddUser(administrator.Guid);
            webEditors.AddUser(editor.Guid);
            this._roles.Add(webEditors);

            SimpleRole webUsers = new SimpleRole("WebUsers");
            webUsers.AddUser(admin.Guid);
            webUsers.AddUser(administrator.Guid);
            webUsers.AddUser(editor.Guid);
            webUsers.AddUser(user.Guid);
            this._roles.Add(webUsers);
        }

        #endregion

        #region Properties

        public virtual IEmailValidator EmailValidator
        {
            get { return this._emailValidator; }
        }

        protected internal virtual StringComparison RoleNameComparison
        {
            get { return _roleNameComparison; }
        }

        public virtual IEnumerable<IRole> Roles
        {
            get { return this.RolesInternal.ToArray(); }
        }

        protected virtual IList<IRole> RolesInternal
        {
            get { return this._roles; }
        }

        protected internal virtual StringComparison UserEmailComparison
        {
            get { return _userEmailComparison; }
        }

        protected internal virtual StringComparison UserNameComparison
        {
            get { return _userNameComparison; }
        }

        public virtual IEnumerable<IUser> Users
        {
            get { return this.UsersInternal.ToArray(); }
        }

        protected virtual IList<IUser> UsersInternal
        {
            get { return this._users; }
        }

        #endregion

        #region Methods

        public virtual void AddRole(IRole role)
        {
            if(role == null)
                throw new ArgumentNullException("role");

            if(role.Guid == null)
                throw new ArgumentException("The role-guid can not be null.", "role");

            if(role.Guid == Guid.Empty)
                throw new ArgumentException("The role-guid can not be empty.", "role");

            if(role.Name == null)
                throw new ArgumentException("The role-name can not be null.", "role");

            if(role.Name.Trim().Length == 0)
                throw new ArgumentException("The role-name can not be empty.", "role");

            if(this.GetRoleByGuid(role.Guid) != null)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "A role with guid \"{0}\" already exists.", role.Guid));

            if(this.GetRoleByName(role.Name) != null)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "A role with name \"{0}\" already exists.", role.Name));

            this.RolesInternal.Add(role);
        }

        public virtual void AddUser(IUser user)
        {
            if(user == null)
                throw new ArgumentNullException("user");

            if(user.Guid == null)
                throw new ArgumentException("The user-guid can not be null.", "user");

            if(user.Guid == Guid.Empty)
                throw new ArgumentException("The user-guid can not be empty.", "user");

            if(user.Name == null)
                throw new ArgumentException("The user-name can not be null.", "user");

            if(user.Name.Trim().Length == 0)
                throw new ArgumentException("The user-name can not be empty.", "user");

            if(user.EmailAddress == null)
                throw new ArgumentException("The email-address can not be null.", "user");

            if(user.EmailAddress.Trim().Length == 0)
                throw new ArgumentException("The email-address can not be empty.", "user");

            if(!this.EmailValidator.IsValidEmailAddress(user.EmailAddress))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The email-address \"{0}\" is not a valid email-address.", user.EmailAddress), "user");

            if(this.GetUserByGuid(user.Guid) != null)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "A user with guid \"{0}\" already exists.", user.Guid));

            if(this.GetUserByName(user.Name) != null)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "A user with name \"{0}\" already exists.", user.Name));

            if(this.GetUserByEmail(user.EmailAddress) != null)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "A user with email-address \"{0}\" already exists.", user.EmailAddress));

            this.UsersInternal.Add(user);
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        public virtual IRole GetRoleByGuid(Guid guid)
        {
            return this.RolesInternal.FirstOrDefault(role => role.Guid.Equals(guid));
        }

        public virtual IRole GetRoleByName(string name)
        {
            return this.RolesInternal.FirstOrDefault(role => role.Name.Equals(name, this.RoleNameComparison));
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        public virtual IEnumerable<IRole> GetRoles(Guid userGuid)
        {
            if(userGuid == null)
                throw new ArgumentNullException("userGuid");

            if(userGuid == Guid.Empty)
                throw new ArgumentException("The user-guid can not be empty.", "userGuid");

            return this.RolesInternal.Where(role => role.Users.Contains(userGuid));
        }

        public virtual IUser GetUserByEmail(string emailAddress)
        {
            return this.UsersInternal.FirstOrDefault(user => user.EmailAddress.Equals(emailAddress, this.UserEmailComparison));
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        public virtual IUser GetUserByGuid(Guid guid)
        {
            return this.UsersInternal.FirstOrDefault(user => user.Guid.Equals(guid));
        }

        public virtual IUser GetUserByName(string name)
        {
            return this.UsersInternal.FirstOrDefault(user => user.Name.Equals(name, this.UserNameComparison));
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        public virtual bool RemoveRole(Guid roleGuid)
        {
            if(roleGuid == null)
                throw new ArgumentNullException("roleGuid");

            if(roleGuid == Guid.Empty)
                throw new ArgumentException("The role-guid can not be empty.", "roleGuid");

            bool removed = false;

            for(int i = this.RolesInternal.Count; i >= 0; i--)
            {
                if(this.RolesInternal[i].Guid != roleGuid)
                    continue;

                this.RolesInternal.RemoveAt(i);
                removed = true;
            }

            return removed;
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        public virtual bool RemoveUser(Guid userGuid)
        {
            if(userGuid == null)
                throw new ArgumentNullException("userGuid");

            if(userGuid == Guid.Empty)
                throw new ArgumentException("The user-guid can not be empty.", "userGuid");

            bool removed = false;

            for(int i = this.UsersInternal.Count; i >= 0; i--)
            {
                if(this.UsersInternal[i].Guid != userGuid)
                    continue;

                this.UsersInternal.RemoveAt(i);
                removed = true;
            }

            foreach(IRole role in this.RolesInternal)
            {
                role.RemoveUser(userGuid);
            }

            return removed;
        }

        #endregion
    }
}