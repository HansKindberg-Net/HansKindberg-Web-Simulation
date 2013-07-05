using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Web;

namespace HansKindberg.Web.Simulation
{
    public class BrowsingSession<TRequestResult> where TRequestResult : RequestResult, new()
    {
        #region Fields

        private readonly HttpCookieCollection _cookies = new HttpCookieCollection();
        private readonly IHttpRuntime _httpRuntime;

        #endregion

        #region Constructors

        public BrowsingSession(IHttpRuntime httpRuntime)
        {
            if(httpRuntime == null)
                throw new ArgumentNullException("httpRuntime");

            this._httpRuntime = httpRuntime;
        }

        #endregion

        #region Properties

        public virtual HttpCookieCollection Cookies
        {
            get { return this._cookies; }
        }

        protected internal virtual IHttpRuntime HttpRuntime
        {
            get { return this._httpRuntime; }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual HttpSessionStateBase Session { get; protected internal set; }

        #endregion

        #region Methods

        protected internal virtual void AddAnyNewCookiesToCookieCollection(TRequestResult requestResult)
        {
            if(requestResult == null)
                throw new ArgumentNullException("requestResult");

            if(requestResult.Response == null)
                return;

            HttpCookieCollection responseCookies = requestResult.Response.Cookies;

            if(responseCookies == null)
                return;

            foreach(string cookieName in responseCookies)
            {
                HttpCookie cookie = responseCookies[cookieName];

                if(this.Cookies[cookieName] != null)
                    this.Cookies.Remove(cookieName);

                // ReSharper disable PossibleNullReferenceException
                if(cookie.Expires == default(DateTime) || cookie.Expires > DateTime.Now)
                    this.Cookies.Add(cookie);
                // ReSharper restore PossibleNullReferenceException
            }
        }

        protected internal virtual void PopulateRequestResult(TRequestResult requestResult) {}

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public virtual TRequestResult ProcessRequest(string url)
        {
            return this.ProcessRequest(url, HttpVerb.Get, new Dictionary<string, string>());
        }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public virtual TRequestResult ProcessRequest(string url, HttpVerb httpVerb, IDictionary<string, string> form)
        {
            return this.ProcessRequest(url, httpVerb, new Dictionary<string, string>(), form);
        }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public virtual TRequestResult ProcessRequest(string url, HttpVerb httpVerb, IDictionary<string, string> headers, IDictionary<string, string> form)
        {
            if(url == null)
                throw new ArgumentNullException("url");

            if(headers == null)
                throw new ArgumentNullException("headers");

            if(form == null)
                throw new ArgumentNullException("form");

            IDictionary<string, string> cookies = new Dictionary<string, string>();

            foreach(string cookieName in this.Cookies)
            {
                HttpCookie cookie = this.Cookies[cookieName];
                cookies.Add(cookieName, cookie != null ? cookie.Value : string.Empty);
            }

            TRequestResult lastRequestResult = RequestResult.Instance<TRequestResult>();

            lastRequestResult.Clear();

            TRequestResult requestResult;

            using(StringWriter output = new StringWriter(CultureInfo.CurrentCulture))
            {
                HttpWorkerRequest workerRequest = new SimulatedWorkerRequest(url, cookies, form, headers, httpVerb, output);
                this.HttpRuntime.ProcessRequest(workerRequest);

                this.AddAnyNewCookiesToCookieCollection(lastRequestResult);
                this.Session = lastRequestResult.Session;

                requestResult = lastRequestResult.Copy<TRequestResult>();

                requestResult.Content = output.ToString();
            }

            this.PopulateRequestResult(requestResult);

            return requestResult;
        }

        #endregion
    }
}