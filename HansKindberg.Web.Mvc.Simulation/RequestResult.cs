using System.Web.Mvc;

namespace HansKindberg.Web.Mvc.Simulation
{
    public class RequestResult : HansKindberg.Web.Simulation.RequestResult
    {
        #region Properties

        public virtual ActionExecutedContext ActionExecutedContext { get; set; }
        public virtual ResultExecutedContext ResultExecutedContext { get; set; }

        #endregion

        #region Methods

        public override void Clear()
        {
            base.Clear();

            this.ActionExecutedContext = null;
            this.ResultExecutedContext = null;
        }

        public override TRequestResult Copy<TRequestResult>()
        {
            TRequestResult copy = base.Copy<TRequestResult>();

            RequestResult requestResult = copy as RequestResult;

            if(requestResult == null)
                return copy;

            requestResult.ActionExecutedContext = this.ActionExecutedContext;
            requestResult.ResultExecutedContext = this.ResultExecutedContext;

            return requestResult as TRequestResult;
        }

        #endregion
    }
}