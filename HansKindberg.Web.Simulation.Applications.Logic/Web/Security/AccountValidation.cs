using System.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Web.Security
{
    public class AccountValidation : IAccountValidation
    {
        #region Methods

        public virtual string GetMessage(MembershipCreateStatus membershipCreateStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for a full list of status codes.
            switch(membershipCreateStatus)
            {
                case MembershipCreateStatus.DuplicateEmail:
                    return "The e-mail address already exists.";
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    return "The provider user key already exists.";
                case MembershipCreateStatus.DuplicateUserName:
                    return "The user name already exists.";
                case MembershipCreateStatus.InvalidAnswer:
                    return "The password answer is not formatted correctly.";
                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address is not formatted correctly.";
                case MembershipCreateStatus.InvalidPassword:
                    return "The password is not formatted correctly.";
                case MembershipCreateStatus.InvalidProviderUserKey:
                    return "The provider user key is of an invalid type or format.";
                case MembershipCreateStatus.InvalidQuestion:
                    return "The password question is not formatted correctly.";
                case MembershipCreateStatus.InvalidUserName:
                    return "The user name was not found.";
                case MembershipCreateStatus.ProviderError:
                    return "The provider returned an error.";
                case MembershipCreateStatus.UserRejected:
                    return "The user was not created, for a reason defined by the provider.";
                default: // MembershipCreateStatus.Success
                    return "The user was successfully created.";
            }
        }

        #endregion
    }
}