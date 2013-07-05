using System.Collections.Generic;
using System.Web.Mvc;

namespace HansKindberg.Web.Mvc.Simulation
{
    public class InterceptionFilterProvider : IFilterProvider
    {
        #region Methods

        public virtual IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            yield return new Filter(new InterceptionFilterAttribute(), FilterScope.Action, null);
        }

        #endregion
    }
}