using System;
using System.Web;

namespace HansKindberg.Web.Simulation
{
    [Serializable]
    public class HttpRuntimeWrapper : IHttpRuntime
    {
        #region Methods

        public virtual void ProcessRequest(HttpWorkerRequest httpWorkerRequest)
        {
            HttpRuntime.ProcessRequest(httpWorkerRequest);
        }

        #endregion
    }
}