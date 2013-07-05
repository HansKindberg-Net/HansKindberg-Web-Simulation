using System.Web.Hosting;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class VirtualApplicationHost : VirtualApplicationHost<RequestResult> {}

    public abstract class VirtualApplicationHost<TRequestResult> : ApplicationHost<TRequestResult> where TRequestResult : RequestResult, new()
    {
        #region Properties

        public virtual SimulatedVirtualPathProvider VirtualPathProvider
        {
            get
            {
                SimulatedVirtualPathProvider simulatedVirtualPathProvider = HostingEnvironment.VirtualPathProvider as SimulatedVirtualPathProvider;

                if(simulatedVirtualPathProvider == null)
                    HostingEnvironment.RegisterVirtualPathProvider(new SimulatedVirtualPathProvider());

                return (SimulatedVirtualPathProvider) HostingEnvironment.VirtualPathProvider;
            }
            set { HostingEnvironment.RegisterVirtualPathProvider(value); }
        }

        #endregion
    }
}