using System;
using System.Web;
using HansKindberg.Web.Simulation.Application.Views.Cookie;
using WebFormsMvp;

namespace HansKindberg.Web.Simulation.Application.Presenters
{
    public class CookiePresenter : Presenter<ICookieView>
    {
        #region Constructors

        public CookiePresenter(ICookieView view) : base(view)
        {
            this.View.Load += this.OnViewLoad;
        }

        #endregion

        #region Eventhandlers

        protected internal virtual void OnViewLoad(object sender, EventArgs e)
        {
            bool overwrite;
            if(!bool.TryParse(this.Request.QueryString["Overwrite"], out overwrite))
                overwrite = false;

            if(overwrite || this.Request.Cookies["FirstCookie"] == null)
                this.Response.Cookies.Add(new HttpCookie("FirstCookie", "FirstCookieValue"));
            if(overwrite || this.Request.Cookies["SecondCookie"] == null)
                this.Response.Cookies.Add(new HttpCookie("SecondCookie", "SecondCookieValue"));
            if(overwrite || this.Request.Cookies["ThirdCookie"] == null)
                this.Response.Cookies.Add(new HttpCookie("ThirdCookie", "ThirdCookieValue"));

            this.Response.Clear();
            this.Response.Write("Getting cookies");
            this.Response.End();
        }

        #endregion
    }
}