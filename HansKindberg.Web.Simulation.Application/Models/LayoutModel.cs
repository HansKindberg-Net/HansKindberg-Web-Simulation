using System;
using System.Web;
using HansKindberg.Web.Simulation.Application.Business.Web.Mvp.UI;

namespace HansKindberg.Web.Simulation.Application.Models
{
    public class LayoutModel
    {
        #region Fields

        private readonly HttpRequestBase _httpRequest;
        private readonly IPageView _pageView;

        #endregion

        #region Constructors

        public LayoutModel(IPageView pageView, HttpRequestBase httpRequest)
        {
            if(pageView == null)
                throw new ArgumentNullException("pageView");

            if(httpRequest == null)
                throw new ArgumentNullException("httpRequest");

            this._httpRequest = httpRequest;
            this._pageView = pageView;
        }

        #endregion

        #region Properties

        public virtual HttpRequestBase HttpRequest
        {
            get { return this._httpRequest; }
        }

        public virtual IPageView PageView
        {
            get { return this._pageView; }
        }

        #endregion
    }
}