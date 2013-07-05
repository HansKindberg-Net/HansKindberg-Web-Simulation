using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using HansKindberg.Web.Simulation.Serialization;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class ApplicationHost : ApplicationHost<RequestResult> {}

    public abstract class ApplicationHost<TRequestResult> : MarshalByRefObject, IDisposable where TRequestResult : RequestResult, new()
    {
        #region Fields

        private IHttpRuntime _httpRuntime;

        #endregion

        #region Events

        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        public event Action<HttpApplicationEvent> AnyApplicationEvent;

        #endregion

        #region Properties

        protected internal virtual IHttpRuntime HttpRuntime
        {
            get { return this._httpRuntime; }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {}

        public virtual void InitializeApplication(IHttpApplicationManager httpApplicationManager, IHttpRuntime httpRuntime)
        {
            if(httpApplicationManager == null)
                throw new ArgumentNullException("httpApplicationManager");

            if(httpRuntime == null)
                throw new ArgumentNullException("httpRuntime");

            this._httpRuntime = httpRuntime;

            HttpApplication httpApplication;

            using(StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                httpApplication = httpApplicationManager.GetApplicationInstance(new SimpleWorkerRequest(string.Empty, string.Empty, stringWriter));
            }

            httpApplication.AcquireRequestState += this.OnAcquireRequestState;
            httpApplication.AuthenticateRequest += this.OnAuthenticateRequest;
            httpApplication.AuthorizeRequest += this.OnAuthorizeRequest;
            httpApplication.BeginRequest += this.OnBeginRequest;
            httpApplication.Disposed += this.OnDisposed;
            httpApplication.EndRequest += this.OnEndRequest;
            httpApplication.Error += this.OnError;
            //httpApplication.LogRequest += this.OnLogRequest; // This operation requires IIS integrated pipeline mode.
            //httpApplication.MapRequestHandler += this.OnMapRequestHandler; // This operation requires IIS integrated pipeline mode.
            httpApplication.PostAcquireRequestState += this.OnPostAcquireRequestState;
            httpApplication.PostAuthenticateRequest += this.OnPostAuthenticateRequest;
            httpApplication.PostAuthorizeRequest += this.OnPostAuthorizeRequest;
            //httpApplication.PostLogRequest += this.OnPostLogRequest; // This operation requires IIS integrated pipeline mode.
            httpApplication.PostMapRequestHandler += this.OnPostMapRequestHandler;
            httpApplication.PostReleaseRequestState += this.OnPostReleaseRequestState;
            httpApplication.PostRequestHandlerExecute += this.OnPostRequestHandlerExecute;
            httpApplication.PostResolveRequestCache += this.OnPostResolveRequestCache;
            httpApplication.PostUpdateRequestCache += this.OnPostUpdateRequestCache;
            httpApplication.PreRequestHandlerExecute += this.OnPreRequestHandlerExecute;
            httpApplication.PreSendRequestContent += this.OnPreSendRequestContent;
            httpApplication.PreSendRequestHeaders += this.OnPreSendRequestHeaders;
            httpApplication.ReleaseRequestState += this.OnReleaseRequestState;
            //httpApplication.RequestCompleted += this.OnRequestCompleted; // Not in .NET Framework 3.5
            httpApplication.ResolveRequestCache += this.OnResolveRequestCache;
            httpApplication.UpdateRequestCache += this.OnUpdateRequestCache;

            httpApplicationManager.RefreshApplicationEventsList(httpApplication);

            httpApplicationManager.RecycleApplicationInstance(httpApplication);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        protected internal virtual void OnAcquireRequestState(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.AcquireRequestState);
        }

        protected internal virtual void OnAnyApplicationEvent(HttpApplicationEvent httpApplicationEvent)
        {
            if(this.AnyApplicationEvent != null)
                this.AnyApplicationEvent(httpApplicationEvent);
        }

        protected internal virtual void OnAuthenticateRequest(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.AuthenticateRequest);
        }

        protected internal virtual void OnAuthorizeRequest(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.AuthorizeRequest);
        }

        protected internal virtual void OnBeginRequest(object sender, EventArgs eventArgs)
        {
            TRequestResult requestResult = RequestResult.Instance<TRequestResult>();

            requestResult.Clear();

            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.BeginRequest);
        }

        protected internal virtual void OnDisposed(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.Disposed);
        }

        protected internal virtual void OnEndRequest(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.EndRequest);
        }

        protected internal virtual void OnError(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.Error);
        }

        protected internal virtual void OnLogRequest(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.LogRequest);
        }

        protected internal virtual void OnMapRequestHandler(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.MapRequestHandler);
        }

        protected internal virtual void OnPostAcquireRequestState(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PostAcquireRequestState);
        }

        protected internal virtual void OnPostAuthenticateRequest(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PostAuthenticateRequest);
        }

        protected internal virtual void OnPostAuthorizeRequest(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PostAuthorizeRequest);
        }

        protected internal virtual void OnPostLogRequest(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PostLogRequest);
        }

        protected internal virtual void OnPostMapRequestHandler(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PostMapRequestHandler);
        }

        protected internal virtual void OnPostReleaseRequestState(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PostReleaseRequestState);
        }

        protected internal virtual void OnPostRequestHandlerExecute(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PostRequestHandlerExecute);
        }

        protected internal virtual void OnPostResolveRequestCache(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PostResolveRequestCache);
        }

        protected internal virtual void OnPostUpdateRequestCache(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PostUpdateRequestCache);
        }

        protected internal virtual void OnPreRequestHandlerExecute(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PreRequestHandlerExecute);
        }

        protected internal virtual void OnPreSendRequestContent(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PreSendRequestContent);
        }

        protected internal virtual void OnPreSendRequestHeaders(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.PreSendRequestHeaders);
        }

        protected internal virtual void OnReleaseRequestState(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.ReleaseRequestState);
        }

        protected internal virtual void OnRequestCompleted(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.RequestCompleted);
        }

        protected internal virtual void OnResolveRequestCache(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.ResolveRequestCache);
        }

        protected internal virtual void OnUpdateRequestCache(object sender, EventArgs eventArgs)
        {
            this.PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent.UpdateRequestCache);
        }

        protected internal virtual void PopulateRequestResult(TRequestResult requestResult, HttpApplicationEvent httpApplicationEvent)
        {
            HttpContextBase httpContext = HttpContext.Current != null ? new HttpContextWrapper(HttpContext.Current) : null;

            if(httpContext != null)
            {
                if(requestResult.Application == null)
                    requestResult.Application = httpContext.Application;

                if(requestResult.Context == null)
                    requestResult.Context = httpContext;

                if(requestResult.Handler == null)
                    requestResult.Handler = httpContext.Handler;

                if(requestResult.Request == null)
                    requestResult.Request = httpContext.Request;

                if(requestResult.Response == null)
                    requestResult.Response = httpContext.Response;

                if(requestResult.Server == null)
                    requestResult.Server = httpContext.Server;

                if(requestResult.Session == null)
                    requestResult.Session = httpContext.Session;
            }

            requestResult.InvokedEvents.Add(httpApplicationEvent);
        }

        protected internal virtual void PopulateRequestResultAndRaiseAnyApplicationEvent(HttpApplicationEvent httpApplicationEvent)
        {
            this.PopulateRequestResult(RequestResult.Instance<TRequestResult>(), httpApplicationEvent);

            this.OnAnyApplicationEvent(httpApplicationEvent);
        }

        public virtual void Run(Action code)
        {
            if(code == null)
                throw new ArgumentNullException("code");

            code();
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public virtual void Run(SerializableDelegate<Action<BrowsingSession<TRequestResult>>> browsingSession)
        {
            if(browsingSession == null)
                throw new ArgumentNullException("browsingSession");

            browsingSession.DelegateInstance(new BrowsingSession<TRequestResult>(this.HttpRuntime));
        }

        #endregion
    }
}