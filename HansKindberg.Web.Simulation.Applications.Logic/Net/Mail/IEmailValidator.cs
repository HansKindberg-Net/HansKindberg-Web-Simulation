namespace HansKindberg.Web.Simulation.Applications.Logic.Net.Mail
{
    public interface IEmailValidator
    {
        #region Methods

        bool IsValidEmailAddress(string emailAddress);
        void ValidateEmailAddress(string emailAddress);

        #endregion
    }
}