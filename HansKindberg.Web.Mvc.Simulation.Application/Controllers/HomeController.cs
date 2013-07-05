using System.Web.Mvc;

namespace HansKindberg.Web.Mvc.Simulation.Application.Controllers
{
    public class HomeController : Controller
    {
        #region Methods

        public virtual ActionResult About()
        {
            return this.View();
        }

        public virtual ActionResult Index()
        {
            this.ViewBag.Message = "Welcome to the sample MVC application for integration testing!";

            return this.View();
        }

        #endregion
    }
}