using System;
using HansKindberg.Web.Simulation.Application.Models;
using HansKindberg.Web.Simulation.Application.Views.About;
using WebFormsMvp;

namespace HansKindberg.Web.Simulation.Application.Presenters
{
    public class AboutPresenter : Presenter<IAboutView>
    {
        #region Constructors

        public AboutPresenter(IAboutView view) : base(view)
        {
            this.View.Load += this.OnViewLoad;
        }

        #endregion

        #region Eventhandlers

        protected internal virtual void OnViewLoad(object sender, EventArgs e)
        {
            this.View.Model = new AboutModel();
        }

        #endregion
    }
}