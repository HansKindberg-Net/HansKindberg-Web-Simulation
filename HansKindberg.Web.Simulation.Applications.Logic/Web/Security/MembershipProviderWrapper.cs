using System;
using System.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Web.Security
{
    public class MembershipProviderWrapper : IMembershipProvider
    {
        #region Fields

        private readonly MembershipProvider _membershipProvider;

        #endregion

        #region Constructors

        public MembershipProviderWrapper(MembershipProvider membershipProvider)
        {
            if(membershipProvider == null)
                throw new ArgumentNullException("membershipProvider");

            this._membershipProvider = membershipProvider;
        }

        #endregion

        #region Properties

        protected internal virtual MembershipProvider MembershipProvider
        {
            get { return this._membershipProvider; }
        }

        public virtual int MinimumRequiredPasswordLength
        {
            get { return this._membershipProvider.MinRequiredPasswordLength; }
        }

        #endregion

        #region Methods

        public virtual bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return this.MembershipProvider.ChangePassword(userName, oldPassword, newPassword);
        }

        public virtual MembershipUser CreateUser(string userName, string password, string email, out MembershipCreateStatus membershipCreateStatus)
        {
            return this.MembershipProvider.CreateUser(userName, password, email, string.Empty, string.Empty, true, null, out membershipCreateStatus);
        }

        public virtual MembershipUser GetUser(string name)
        {
            return this.MembershipProvider.GetUser(name, false);
        }

        public virtual bool ValidateUser(string userName, string password)
        {
            return this.MembershipProvider.ValidateUser(userName, password);
        }

        #endregion
    }
}