using System.Web.Mvc;

namespace HansKindberg.Web.Mvc.Simulation.Application.Controllers
{
    public class CurrentUserController : Controller
    {
        #region Methods

        [Authorize]
        public virtual ActionResult WhoAreYou()
        {
            return this.Content("Hello, you're logged in as " + this.User.Identity.Name);
        }

        #endregion
    }
}