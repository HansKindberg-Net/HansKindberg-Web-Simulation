using System;
using System.Web.Mvc;

namespace HansKindberg.Web.Mvc.Simulation.Application.Business.Configuration
{
    public static class FilterConfiguration
    {
        #region Methods

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if(filters == null)
                throw new ArgumentNullException("filters");

            filters.Add(new HandleErrorAttribute());
        }

        #endregion
    }
}