using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace HansKindberg.Web.Simulation
{
    public class SimulatedWorkerRequest : SimpleWorkerRequest
    {
        #region Fields

        public const string PhysicalApplicationDirectoryPathDomainPropertyName = ".appPath";
        public const string VirtualApplicationDirectoryPathDomainPropertyName = ".appVPath";
        private readonly IDictionary<string, string> _cookies;
        private const string _defaultHostName = "hanskindberg.web.simulation";
        private const int _defaultPort = 80;
        private readonly string _filePath;
        private string _filePathTranslated;
        private readonly IDictionary<string, string> _form;
        private readonly IDictionary<string, string> _headers;
        private readonly HttpVerb _httpVerb;
        private readonly string _pathInfo;
        private readonly Uri _uri;

        #endregion

        #region Constructors

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public SimulatedWorkerRequest(string url, TextWriter output) : this(url, new Dictionary<string, string>(), new Dictionary<string, string>(), new Dictionary<string, string>(), HttpVerb.Get, output) {}

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public SimulatedWorkerRequest(string url, IDictionary<string, string> cookies, IDictionary<string, string> form, IDictionary<string, string> headers, HttpVerb httpVerb, TextWriter output) : base(ValidateApplicationDomainPropertiesBeforePassingPageParameter(string.Empty), string.Empty, output)
        {
            if(url == null)
                throw new ArgumentNullException("url");

            if(cookies == null)
                throw new ArgumentNullException("cookies");

            if(form == null)
                throw new ArgumentNullException("form");

            if(headers == null)
                throw new ArgumentNullException("headers");

            if(output == null)
                throw new ArgumentNullException("output");

            this._cookies = cookies;
            this._form = form;
            this._headers = headers;
            this._httpVerb = httpVerb;

            Uri uri;
            try
            {
                uri = new Uri(url, UriKind.RelativeOrAbsolute);
            }
            catch(Exception exception)
            {
                throw new ArgumentException("The url could not be used to create an uri.", "url", exception);
            }

            if(!uri.IsAbsoluteUri)
            {
                if(!url.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                    url = "/" + url;

                uri = new Uri("http://" + _defaultHostName + ":" + _defaultPort + url);
            }

            this._uri = uri;

            int lastIndexOfDot = uri.LocalPath.LastIndexOf('.');
            int lastIndexOfSlash = uri.LocalPath.LastIndexOf('/');

            if((lastIndexOfDot >= 0 && lastIndexOfSlash >= 0) && lastIndexOfDot < lastIndexOfSlash)
            {
                int length = uri.LocalPath.IndexOf('/', lastIndexOfDot);
                this._filePath = uri.LocalPath.Substring(0, length);
                this._pathInfo = uri.LocalPath.Substring(length);
            }
            else
            {
                this._filePath = uri.LocalPath;
                this._pathInfo = string.Empty;
            }
        }

        #endregion

        #region Properties

        protected internal virtual IDictionary<string, string> Cookies
        {
            get { return this._cookies; }
        }

        protected internal virtual IDictionary<string, string> Form
        {
            get { return this._form; }
        }

        protected internal virtual IDictionary<string, string> Headers
        {
            get { return this._headers; }
        }

        protected internal virtual HttpVerb HttpVerb
        {
            get { return this._httpVerb; }
        }

        #endregion

        #region Methods

        public override string GetFilePath()
        {
            return this._filePath;
        }

        public override string GetFilePathTranslated()
        {
            return this._filePathTranslated ?? (this._filePathTranslated = (this.GetAppPathTranslated() ?? string.Empty).TrimEnd("\\".ToCharArray()) + this.GetFilePath().Replace('/', '\\'));
        }

        public override string GetHttpVerbName()
        {
            return this.HttpVerb.ToString().ToUpperInvariant();
        }

        public override string GetKnownRequestHeader(int index)
        {
            // Override "Content-Type" header for POST requests, otherwise ASP.NET won't read the Form collection
            if(index == HeaderContentType && this.HttpVerb == HttpVerb.Post) // HeaderContentType = 12
                return "application/x-www-form-urlencoded";

            if(index == HeaderRetryAfter) // HeaderRetryAfter = 0x19
                return this.MakeCookieHeader();

            string headerName = GetKnownRequestHeaderName(index);

            return (headerName != null && this.Headers.ContainsKey(headerName)) ? this.Headers[headerName] : null;
        }

        public override string GetLocalAddress()
        {
            return this._uri.Host;
        }

        public override int GetLocalPort()
        {
            return this._uri.Port;
        }

        public override string GetPathInfo()
        {
            return this._pathInfo;
        }

        public override byte[] GetPreloadedEntityBody()
        {
            if(!this.Form.Any())
                return base.GetPreloadedEntityBody();

            var stringBuilder = new StringBuilder();

            foreach(KeyValuePair<string, string> item in this.Form)
            {
                stringBuilder.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(item.Key), HttpUtility.UrlEncode(item.Value));
            }

            return Encoding.UTF8.GetBytes(stringBuilder.ToString());
        }

        public override string GetQueryString()
        {
            return this._uri.Query.TrimStart("?".ToCharArray());
        }

        public override string GetRawUrl()
        {
            return this._uri.PathAndQuery;
        }

        public override string GetUnknownRequestHeader(string name)
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            return (name != null && this.Headers.ContainsKey(name)) ? this.Headers[name] : null;
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        public override string[][] GetUnknownRequestHeaders()
        {
            if(!this.Headers.Any())
                return null;

            var unknownHeaders = from key in this.Headers.Keys
                                 let knownRequestHeaderIndex = GetKnownRequestHeaderIndex(key)
                                 where knownRequestHeaderIndex < 0
                                 select new[] {key, this.Headers[key]};

            return unknownHeaders.ToArray();
        }

        public override string GetUriPath()
        {
            return this._uri.LocalPath;
        }

        protected internal virtual string MakeCookieHeader()
        {
            if(!this.Cookies.Any())
                return null;

            var stringBuilder = new StringBuilder();

            foreach(KeyValuePair<string, string> item in this.Cookies)
            {
                stringBuilder.AppendFormat("{0}={1};", item.Key, item.Value);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// This method validates the application domain properties that are used in the System.Web.Hosting.SimpleWorkerRequest constructor and throws a more explanatory exception if any of them is null.
        /// The System.Web.Hosting.SimpleWorkerRequest constructor only throws a NullReferenceException if any of them is null.
        /// </summary>
        /// <param name="page"></param>
        /// <returns>The same value as passed in. The method is only for validation.</returns>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "appPath")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "appVPath")]
        public static string ValidateApplicationDomainPropertiesBeforePassingPageParameter(string page)
        {
            if(Thread.GetDomain().GetData(VirtualApplicationDirectoryPathDomainPropertyName) == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The path for the virtual application directory for the current domain is not set. The name of the domain property is \"{0}\".", VirtualApplicationDirectoryPathDomainPropertyName));

            if(Thread.GetDomain().GetData(PhysicalApplicationDirectoryPathDomainPropertyName) == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The path for the physical application directory for the current domain is not set. The name of the domain property is \"{0}\".", PhysicalApplicationDirectoryPathDomainPropertyName));

            return page;
        }

        #endregion
    }
}