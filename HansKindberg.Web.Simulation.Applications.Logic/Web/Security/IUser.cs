namespace HansKindberg.Web.Simulation.Applications.Logic.Web.Security
{
    public interface IUser : IPrincipal
    {
        #region Properties

        string EmailAddress { get; }
        string Password { get; set; }

        #endregion
    }
}