using System;
using HansKindberg.Web.Simulation.Application.Models;
using HansKindberg.Web.Simulation.Application.Views.Default;
using WebFormsMvp;

namespace HansKindberg.Web.Simulation.Application.Presenters
{
    public class DefaultPresenter : Presenter<IDefaultView>
    {
        #region Constructors

        public DefaultPresenter(IDefaultView view) : base(view)
        {
            this.View.Load += this.OnViewLoad;
        }

        #endregion

        #region Eventhandlers

        protected internal virtual void OnViewLoad(object sender, EventArgs e)
        {
            this.View.Model = new DefaultModel();
        }

        #endregion
    }
}