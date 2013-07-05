using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace HansKindberg.Web.Simulation
{
    public class RequestResult
    {
        #region Fields

        private static readonly IDictionary<Type, RequestResult> _instances = new Dictionary<Type, RequestResult>();
        private readonly ICollection<HttpApplicationEvent> _invokedEvents = new List<HttpApplicationEvent>();
        private static readonly object _lockObject = new object();

        #endregion

        #region Properties

        public virtual IEnumerable<Exception> AllExceptions
        {
            get { return this.Context == null ? new Exception[0] : this.Context.AllErrors; }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual HttpApplicationStateBase Application { get; set; }

        public virtual string Content { get; set; }
        public virtual HttpContextBase Context { get; set; }

        public virtual HttpApplicationEvent? FirstInvokedEvent
        {
            get
            {
                if(!this.InvokedEvents.Any())
                    return null;

                return this.InvokedEvents.First();
            }
        }

        public virtual IHttpHandler Handler { get; set; }

        public virtual ICollection<HttpApplicationEvent> InvokedEvents
        {
            get { return this._invokedEvents; }
        }

        public virtual Exception LastException
        {
            get { return this.Server == null ? null : this.Server.GetLastError(); }
        }

        public virtual HttpApplicationEvent? LastInvokedEvent
        {
            get
            {
                if(!this.InvokedEvents.Any())
                    return null;

                return this.InvokedEvents.Last();
            }
        }

        public virtual HttpRequestBase Request { get; set; }
        public virtual HttpResponseBase Response { get; set; }
        public virtual HttpServerUtilityBase Server { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual HttpSessionStateBase Session { get; set; }

        #endregion

        #region Methods

        public virtual void Clear()
        {
            this.Application = null;
            this.Content = null;
            this.Context = null;
            this.Handler = null;
            this.InvokedEvents.Clear();
            this.Request = null;
            this.Response = null;
            this.Server = null;
            this.Session = null;
        }

        public virtual TRequestResult Copy<TRequestResult>() where TRequestResult : RequestResult, new()
        {
            TRequestResult requestResult = new TRequestResult
                {
                    Application = this.Application,
                    Content = this.Content,
                    Context = this.Context,
                    Handler = this.Handler,
                    Request = this.Request,
                    Response = this.Response,
                    Server = this.Server,
                    Session = this.Session
                };

            foreach(HttpApplicationEvent httpApplicationEvent in this.InvokedEvents)
            {
                requestResult.InvokedEvents.Add(httpApplicationEvent);
            }

            return requestResult;
        }

        public static TRequestResult Instance<TRequestResult>() where TRequestResult : RequestResult, new()
        {
            Type type = typeof(TRequestResult);

            RequestResult instance;

            if(!_instances.TryGetValue(type, out instance))
            {
                lock(_lockObject)
                {
                    if(!_instances.TryGetValue(type, out instance))
                    {
                        instance = new TRequestResult();
                        _instances.Add(type, instance);
                    }
                }
            }

            return (TRequestResult) instance;
        }

        #endregion
    }
}