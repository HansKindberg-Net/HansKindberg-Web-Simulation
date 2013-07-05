using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using HansKindberg.Web.Simulation.Applications.Logic.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Fakes.Web.Security
{
    public class SimpleRole : SimplePrincipal, IRole
    {
        #region Fields

        private readonly IList<Guid> _users = new List<Guid>();

        #endregion

        #region Constructors

        public SimpleRole(string name) : base(name) {}

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        public SimpleRole(Guid guid, string name) : base(guid, name) {}

        #endregion

        #region Properties

        public virtual IEnumerable<Guid> Users
        {
            get { return this.UsersInternal.ToArray(); }
        }

        protected virtual IList<Guid> UsersInternal
        {
            get { return this._users; }
        }

        #endregion

        #region Methods

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        public virtual void AddUser(Guid userGuid)
        {
            if(userGuid == null)
                throw new ArgumentNullException("userGuid");

            if(userGuid == Guid.Empty)
                throw new ArgumentException("The user guid can not be empty.", "userGuid");

            if(this.UsersInternal.Contains(userGuid))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The user guid \"{0}\" already exists.", userGuid));

            this.UsersInternal.Add(userGuid);
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        public virtual bool RemoveUser(Guid userGuid)
        {
            if(userGuid == null)
                throw new ArgumentNullException("userGuid");

            if(userGuid == Guid.Empty)
                throw new ArgumentException("The user guid can not be empty.", "userGuid");

            bool userRemoved = false;

            for(int i = this.UsersInternal.Count; i >= 0; i--)
            {
                if(this.UsersInternal[i] != userGuid)
                    continue;

                this.UsersInternal.RemoveAt(i);
                userRemoved = true;
            }

            return userRemoved;
        }

        #endregion
    }
}