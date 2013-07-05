using System;
using System.Web;
using HansKindberg.Web.Simulation.Application.Views.Session;
using WebFormsMvp;

namespace HansKindberg.Web.Simulation.Application.Presenters
{
    public class SessionPresenter : Presenter<ISessionView>
    {
        #region Constructors

        public SessionPresenter(ISessionView view) : base(view)
        {
            this.View.Load += this.OnViewLoad;
        }

        #endregion

        #region Eventhandlers

        protected internal virtual void OnViewLoad(object sender, EventArgs e)
        {
            if(this.HttpContext == null)
                return;

            HttpSessionStateBase session = this.HttpContext.Session;

            if(session == null)
                return;

            if(session["FirstSession"] == null)
                session["FirstSession"] = string.Empty;
            if(session["SecondSession"] == null)
                session["SecondSession"] = 10;
            if(session["ThirdSession"] == null)
                session["ThirdSession"] = true;

            session["IncrementSession"] = ((int?) (session["IncrementSession"] ?? 0)) + 1;

            this.Response.Clear();
            this.Response.Write("Getting sessions");
            this.Response.End();
        }

        #endregion
    }
}