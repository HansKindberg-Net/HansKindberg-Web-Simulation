using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Security;
using HansKindberg.Web.Simulation.Applications.Logic.Extensions;
using HansKindberg.Web.Simulation.Applications.Logic.IoC;
using HansKindberg.Web.Simulation.Applications.Logic.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Fakes.Web.Security
{
    public class FakedMembershipProvider : System.Web.Security.MembershipProvider
    {
        #region Fields

        private const StringComparison _passwordComparison = StringComparison.Ordinal;
        private readonly IPrincipalRepository _principalRepository;

        #endregion

        #region Constructors

        public FakedMembershipProvider() : this(DependencyResolver.Instance.GetService<IPrincipalRepository>()) {}

        public FakedMembershipProvider(IPrincipalRepository principalRepository)
        {
            if(principalRepository == null)
                throw new ArgumentNullException("principalRepository");

            this._principalRepository = principalRepository;
        }

        #endregion

        #region Properties

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return int.MaxValue; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 1; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 10; }
        }

        protected internal virtual StringComparison PasswordComparison
        {
            get { return _passwordComparison; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Clear; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return null; }
        }

        protected internal virtual IPrincipalRepository PrincipalRepository
        {
            get { return this._principalRepository; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        #endregion

        #region Methods

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            IUser user;
            if(!this.ValidateUser(username, oldPassword, out user))
                throw new InvalidOperationException("The username or the old password is incorrect.");

            this.ValidatePassword(newPassword, "newPassword");

            user.Password = newPassword;

            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        protected internal virtual MembershipUser CreateMembershipUser(IUser user)
        {
            if(user == null)
                throw new ArgumentNullException("user");

            return new MembershipUser(this.Name, user.Name, user.Guid, user.EmailAddress, null, null, true, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            IUser user;
            Guid? userGuid = null;

            if(providerUserKey != null)
            {
                userGuid = providerUserKey as Guid?;
                if(userGuid == null || userGuid == Guid.Empty)
                {
                    status = MembershipCreateStatus.InvalidProviderUserKey;
                    return null;
                }

                user = this.PrincipalRepository.GetUserByGuid(userGuid.Value);
                if(user != null)
                {
                    status = MembershipCreateStatus.DuplicateProviderUserKey;
                    return null;
                }
            }

            if(string.IsNullOrEmpty(username))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }

            user = this.PrincipalRepository.GetUserByName(username);
            if(user != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            if(string.IsNullOrEmpty(password) || password.Length < this.MinRequiredPasswordLength)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if(!this.PrincipalRepository.EmailValidator.IsValidEmailAddress(email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            user = this.PrincipalRepository.GetUserByEmail(email);
            if(user != null)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            try
            {
                user = userGuid.HasValue ? new SimpleUser(userGuid.Value, username, password, email) : new SimpleUser(username, password, email);
                this.PrincipalRepository.AddUser(user);
            }
            catch
            {
                status = MembershipCreateStatus.ProviderError;
                return null;
            }

            status = MembershipCreateStatus.Success;
            return this.CreateMembershipUser(user);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            if(usernameToMatch == null)
                throw new ArgumentNullException("usernameToMatch");

            IEnumerable<IUser> users = this.PrincipalRepository.Users.Where(user => user.Name != null && user.Name.Like(usernameToMatch.Replace("%", "*"))).ToArray();

            totalRecords = users.Count();

            MembershipUserCollection membershipUsers = new MembershipUserCollection();

            foreach(MembershipUser membershipUser in users.Skip(pageIndex*pageSize).Take(pageSize).Select(this.CreateMembershipUser))
            {
                membershipUsers.Add(membershipUser);
            }

            return membershipUsers;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection selectedUsers = new MembershipUserCollection();
            IEnumerable<IUser> allUsers = this.PrincipalRepository.Users.OrderBy(user => user.Name).ToArray();
            totalRecords = allUsers.Count();

            foreach(IUser user in allUsers.Skip(pageIndex).Take(pageSize))
            {
                selectedUsers.Add(this.CreateMembershipUser(user));
            }

            return selectedUsers;
        }

        protected internal virtual IEnumerable<MembershipUser> GetAllUsers()
        {
            return this.PrincipalRepository.Users.OrderBy(user => user.Name).Select(this.CreateMembershipUser).ToArray();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            IUser user = this.PrincipalRepository.GetUserByGuid((Guid) providerUserKey);

            return user != null ? this.CreateMembershipUser(user) : null;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            IUser user = this.PrincipalRepository.GetUserByName(username);

            return user != null ? this.CreateMembershipUser(user) : null;
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        protected internal virtual IUser GetValidatedUser(string userName, string password)
        {
            if(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return null;

            IUser user = this.PrincipalRepository.GetUserByName(userName);

            if(user == null)
                return null;

            return user.Password.Equals(password, this.PasswordComparison) ? user : null;
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        protected internal virtual void ValidatePassword(string password)
        {
            this.ValidatePassword(password, "password");
        }

        protected internal virtual void ValidatePassword(string password, string parameterName)
        {
            if(password == null)
                throw new ArgumentNullException(parameterName);

            if(password.Length < this.MinRequiredPasswordLength)
                throw new ArgumentException("The password is to short.", parameterName);
        }

        public override bool ValidateUser(string username, string password)
        {
            return this.GetValidatedUser(username, password) != null;
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        protected internal virtual bool ValidateUser(string userName, string password, out IUser user)
        {
            user = this.GetValidatedUser(userName, password);

            return user != null;
        }

        #endregion
    }
}