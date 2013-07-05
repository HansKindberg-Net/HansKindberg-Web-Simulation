using System.Web;

namespace HansKindberg.Web.Simulation
{
    public interface IHttpRuntime
    {
        #region Methods

        void ProcessRequest(HttpWorkerRequest httpWorkerRequest);

        #endregion
    }
}