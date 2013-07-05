using System.Diagnostics.CodeAnalysis;
using System.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Web.Security
{
    public interface IMembershipProvider
    {
        #region Properties

        int MinimumRequiredPasswordLength { get; }

        #endregion

        #region Methods

        bool ChangePassword(string userName, string oldPassword, string newPassword);

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        MembershipUser CreateUser(string userName, string password, string email, out MembershipCreateStatus membershipCreateStatus);

        MembershipUser GetUser(string name);
        bool ValidateUser(string userName, string password);

        #endregion
    }
}