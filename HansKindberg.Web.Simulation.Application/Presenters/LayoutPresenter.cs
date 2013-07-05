using System;
using HansKindberg.Web.Simulation.Application.Models;
using HansKindberg.Web.Simulation.Application.Views.Shared;
using WebFormsMvp;

namespace HansKindberg.Web.Simulation.Application.Presenters
{
    public class LayoutPresenter : Presenter<ILayoutView>
    {
        #region Constructors

        public LayoutPresenter(ILayoutView view) : base(view)
        {
            this.View.Load += this.OnViewLoad;
        }

        #endregion

        #region Eventhandlers

        protected internal virtual void OnViewLoad(object sender, EventArgs e)
        {
            this.View.Model = new LayoutModel(this.View.PageView, this.Request);
            this.View.HeadControl.DataBind();
            this.View.NavigationControl.DataBind();
            this.View.HeadingControl.DataBind();
        }

        #endregion
    }
}