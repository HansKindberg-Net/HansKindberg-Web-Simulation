using System;
using System.Web.Mvc;

namespace HansKindberg.Web.Mvc.Simulation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class InterceptionFilterAttribute : ActionFilterAttribute
    {
        #region Methods

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if(filterContext == null)
                return;

            RequestResult requestResult = HansKindberg.Web.Simulation.RequestResult.Instance<RequestResult>();

            if(requestResult.ActionExecutedContext == null)
            {
                requestResult.ActionExecutedContext = new ActionExecutedContext
                    {
                        ActionDescriptor = filterContext.ActionDescriptor,
                        Canceled = filterContext.Canceled,
                        Controller = filterContext.Controller,
                        Exception = filterContext.Exception,
                        ExceptionHandled = filterContext.ExceptionHandled,
                        HttpContext = filterContext.HttpContext,
                        RequestContext = filterContext.RequestContext,
                        Result = filterContext.Result,
                        RouteData = filterContext.RouteData
                    };
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if(filterContext == null)
                return;

            RequestResult requestResult = HansKindberg.Web.Simulation.RequestResult.Instance<RequestResult>();

            if(requestResult.ResultExecutedContext == null)
                requestResult.ResultExecutedContext = new ResultExecutedContext
                    {
                        Canceled = filterContext.Canceled,
                        Exception = filterContext.Exception,
                        Controller = filterContext.Controller,
                        ExceptionHandled = filterContext.ExceptionHandled,
                        HttpContext = filterContext.HttpContext,
                        RequestContext = filterContext.RequestContext,
                        Result = filterContext.Result,
                        RouteData = filterContext.RouteData
                    };
        }

        #endregion
    }
}