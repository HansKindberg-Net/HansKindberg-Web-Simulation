using System;
using System.Diagnostics.CodeAnalysis;
using HansKindberg.Web.Simulation.Applications.Logic.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Fakes.Web.Security
{
    public class SimpleUser : SimplePrincipal, IUser
    {
        #region Fields

        private readonly string _emailAddress;
        private string _password;

        #endregion

        #region Constructors

        public SimpleUser(string name, string password, string emailAddress) : this(Guid.NewGuid(), name, password, emailAddress) {}

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        public SimpleUser(Guid guid, string name, string password, string emailAddress) : base(guid, name)
        {
            ValidatePassword(password);

            if(emailAddress == null)
                throw new ArgumentNullException("emailAddress");

            if(emailAddress.Trim().Length == 0)
                throw new ArgumentException("The email-address can not be empty.", "emailAddress");

            this._emailAddress = emailAddress;
            this._password = password;
        }

        #endregion

        #region Properties

        public virtual string EmailAddress
        {
            get { return this._emailAddress; }
        }

        public virtual string Password
        {
            get { return this._password; }
            set
            {
                ValidatePassword(value, "value");
                this._password = value;
            }
        }

        #endregion

        #region Methods

        protected static void ValidatePassword(string password)
        {
            ValidatePassword(password, "password");
        }

        protected static void ValidatePassword(string password, string parameterName)
        {
            if(password == null)
                throw new ArgumentNullException(parameterName);

            if(password.Trim().Length == 0)
                throw new ArgumentException("The password can not be empty.", parameterName);
        }

        #endregion
    }
}