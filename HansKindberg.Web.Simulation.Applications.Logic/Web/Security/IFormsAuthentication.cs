namespace HansKindberg.Web.Simulation.Applications.Logic.Web.Security
{
    public interface IFormsAuthentication
    {
        #region Methods

        void RedirectFromLogOnPage(string userName, bool createPersistentCookie);
        void SetAuthenticationCookie(string userName, bool createPersistentCookie);
        void SignOut();

        #endregion
    }
}