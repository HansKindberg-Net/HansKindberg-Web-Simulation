using System.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Web.Security
{
    public interface IAccountValidation
    {
        #region Methods

        string GetMessage(MembershipCreateStatus membershipCreateStatus);

        #endregion
    }
}