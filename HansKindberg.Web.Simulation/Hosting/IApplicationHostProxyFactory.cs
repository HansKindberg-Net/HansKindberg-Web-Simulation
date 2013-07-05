namespace HansKindberg.Web.Simulation.Hosting
{
    public interface IApplicationHostProxyFactory
    {
        #region Methods

        ApplicationHostProxy Create(string physicalDirectoryPath);
        ApplicationHostProxy Create(string physicalDirectoryPath, string virtualPath);

        #endregion
    }
}