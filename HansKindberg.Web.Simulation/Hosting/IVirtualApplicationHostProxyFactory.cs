namespace HansKindberg.Web.Simulation.Hosting
{
    public interface IVirtualApplicationHostProxyFactory
    {
        #region Methods

        VirtualApplicationHostProxy Create();
        VirtualApplicationHostProxy Create(string virtualPath);
        VirtualApplicationHostProxy Create(IFileTransfer fileTransfer);
        VirtualApplicationHostProxy Create(SimulatedVirtualPathProvider virtualPathProvider);
        VirtualApplicationHostProxy Create(string virtualPath, IFileTransfer fileTransfer);
        VirtualApplicationHostProxy Create(string virtualPath, SimulatedVirtualPathProvider virtualPathProvider);
        VirtualApplicationHostProxy Create(IFileTransfer fileTransfer, SimulatedVirtualPathProvider virtualPathProvider);
        VirtualApplicationHostProxy Create(string virtualPath, IFileTransfer fileTransfer, SimulatedVirtualPathProvider virtualPathProvider);

        #endregion
    }
}