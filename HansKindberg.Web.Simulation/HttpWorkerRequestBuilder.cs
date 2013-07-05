using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web;

namespace HansKindberg.Web.Simulation
{
    public class HttpWorkerRequestBuilder
    {
        #region Fields

        private readonly IDictionary<string, string> _cookies = new Dictionary<string, string>();
        private readonly IDictionary<string, string> _form = new Dictionary<string, string>();
        private readonly IDictionary<string, string> _headers = new Dictionary<string, string>();
        private HttpVerb _httpVerb = HttpVerb.Get;
        private string _url = string.Empty;

        #endregion

        #region Properties

        public virtual IDictionary<string, string> Cookies
        {
            get { return this._cookies; }
        }

        public virtual IDictionary<string, string> Form
        {
            get { return this._form; }
        }

        public virtual IDictionary<string, string> Headers
        {
            get { return this._headers; }
        }

        public virtual HttpVerb HttpVerb
        {
            get { return this._httpVerb; }
            set { this._httpVerb = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public virtual string Url
        {
            get { return this._url; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                this._url = value;
            }
        }

        #endregion

        #region Methods

        public virtual HttpWorkerRequest BuildHttpWorkerRequest(TextWriter output)
        {
            if(output == null)
                throw new ArgumentNullException("output");

            return new SimulatedWorkerRequest(this.Url, this.Cookies, this.Form, this.Headers, this.HttpVerb, output);
        }

        #endregion
    }
}