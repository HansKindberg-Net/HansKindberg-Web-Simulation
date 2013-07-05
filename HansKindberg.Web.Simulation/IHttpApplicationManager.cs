using System.Web;

namespace HansKindberg.Web.Simulation
{
    public interface IHttpApplicationManager
    {
        #region Methods

        HttpApplication GetApplicationInstance(HttpWorkerRequest httpWorkerRequest);
        void RecycleApplicationInstance(HttpApplication httpApplication);
        void RefreshApplicationEventsList(HttpApplication httpApplication);

        #endregion
    }
}