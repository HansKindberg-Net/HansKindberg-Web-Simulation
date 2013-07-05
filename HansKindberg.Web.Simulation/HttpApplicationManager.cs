using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Web;

namespace HansKindberg.Web.Simulation
{
    [Serializable]
    public class HttpApplicationManager : IHttpApplicationManager
    {
        #region Fields

        private static readonly IDictionary<HttpApplication, Action<WaitCallback>> _buildStepsDelegates = new Dictionary<HttpApplication, Action<WaitCallback>>();
        private static volatile Func<HttpContext, IHttpHandler> _getApplicationInstanceDelegate;
        private static readonly Type _httpApplicationFactoryType = typeof(HttpContext).Assembly.GetType("System.Web.HttpApplicationFactory", true);
        private static readonly object _lockObject = new object();
        private static volatile Action<HttpApplication> _recycleApplicationInstanceDelegate;
        private static readonly FieldInfo _resumeStepsWaitCallbackField = typeof(HttpApplication).GetField("_resumeStepsWaitCallback", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo _stepManagerField = typeof(HttpApplication).GetField("_stepManager", BindingFlags.Instance | BindingFlags.NonPublic);

        #endregion

        #region Methods

        public virtual HttpApplication GetApplicationInstance(HttpWorkerRequest httpWorkerRequest)
        {
            if(httpWorkerRequest == null)
                throw new ArgumentNullException("httpWorkerRequest");

            if(_getApplicationInstanceDelegate == null)
            {
                lock(_lockObject)
                {
                    if(_getApplicationInstanceDelegate == null)
                    {
                        MethodInfo getApplicationInstanceMethod = _httpApplicationFactoryType.GetMethod("GetApplicationInstance", BindingFlags.NonPublic | BindingFlags.Static);
                        _getApplicationInstanceDelegate = (Func<HttpContext, IHttpHandler>) Delegate.CreateDelegate(typeof(Func<HttpContext, IHttpHandler>), getApplicationInstanceMethod);
                    }
                }
            }

            return (HttpApplication) _getApplicationInstanceDelegate.Invoke(new HttpContext(httpWorkerRequest));
        }

        public virtual void RecycleApplicationInstance(HttpApplication httpApplication)
        {
            if(_recycleApplicationInstanceDelegate == null)
            {
                lock(_lockObject)
                {
                    if(_recycleApplicationInstanceDelegate == null)
                    {
                        MethodInfo recycleApplicationInstanceMethod = _httpApplicationFactoryType.GetMethod("RecycleApplicationInstance", BindingFlags.NonPublic | BindingFlags.Static);
                        _recycleApplicationInstanceDelegate = (Action<HttpApplication>) Delegate.CreateDelegate(typeof(Action<HttpApplication>), recycleApplicationInstanceMethod);
                    }
                }
            }

            _recycleApplicationInstanceDelegate.Invoke(httpApplication);
        }

        public virtual void RefreshApplicationEventsList(HttpApplication httpApplication)
        {
            Action<WaitCallback> buildStepsDelegate;

            if(!_buildStepsDelegates.TryGetValue(httpApplication, out buildStepsDelegate))
            {
                lock(_lockObject)
                {
                    if(!_buildStepsDelegates.TryGetValue(httpApplication, out buildStepsDelegate))
                    {
                        object stepManager = _stepManagerField.GetValue(httpApplication);
                        //object resumeStepsWaitCallback = _resumeStepsWaitCallbackField.GetValue(httpApplication);
                        MethodInfo buildStepsMethod = stepManager.GetType().GetMethod("BuildSteps", BindingFlags.NonPublic | BindingFlags.Instance);
                        buildStepsDelegate = (Action<WaitCallback>) Delegate.CreateDelegate(typeof(Action<WaitCallback>), stepManager, buildStepsMethod);
                        _buildStepsDelegates.Add(httpApplication, buildStepsDelegate);
                    }
                }
            }

            buildStepsDelegate.Invoke((WaitCallback) _resumeStepsWaitCallbackField.GetValue(httpApplication));
        }

        #endregion
    }
}