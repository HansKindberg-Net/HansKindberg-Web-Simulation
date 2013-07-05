namespace HansKindberg.Web.Mvc.Simulation.Hosting
{
    public interface IMvcApplicationHostProxyFactory
    {
        #region Methods

        MvcApplicationHostProxy Create(string physicalDirectoryPath);
        MvcApplicationHostProxy Create(string physicalDirectoryPath, string virtualPath);

        #endregion
    }
}