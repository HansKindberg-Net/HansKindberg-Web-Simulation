using System.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Web.Security
{
    public class FormsAuthenticationWrapper : IFormsAuthentication
    {
        #region Methods

        public virtual void RedirectFromLogOnPage(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.RedirectFromLoginPage(userName, createPersistentCookie);
        }

        public virtual void SetAuthenticationCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public virtual void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        #endregion
    }
}