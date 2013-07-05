using System;
using HansKindberg.Web.Simulation.Application.Views.CurrentUser;
using WebFormsMvp;

namespace HansKindberg.Web.Simulation.Application.Presenters
{
    public class CurrentUserPresenter : Presenter<ICurrentUserView>
    {
        #region Constructors

        public CurrentUserPresenter(ICurrentUserView view) : base(view)
        {
            this.View.Load += this.OnViewLoad;
        }

        #endregion

        #region Eventhandlers

        protected internal virtual void OnViewLoad(object sender, EventArgs e) {}

        #endregion

        //[Authorize]
        //public virtual ActionResult WhoAreYou()
        //{
        //    return this.Content("Hello, you're logged in as " + this.User.Identity.Name);
        //}
    }
}