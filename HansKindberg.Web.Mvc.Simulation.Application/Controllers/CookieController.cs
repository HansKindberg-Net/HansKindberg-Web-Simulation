using System.Web;
using System.Web.Mvc;

namespace HansKindberg.Web.Mvc.Simulation.Application.Controllers
{
    public class CookieController : Controller
    {
        #region Methods

        public virtual ActionResult GetCookies(bool? overwrite)
        {
            if((overwrite.HasValue && overwrite.Value) || this.Request.Cookies["FirstCookie"] == null)
                this.Response.Cookies.Add(new HttpCookie("FirstCookie", "FirstCookieValue"));

            if((overwrite.HasValue && overwrite.Value) || this.Request.Cookies["SecondCookie"] == null)
                this.Response.Cookies.Add(new HttpCookie("SecondCookie", "SecondCookieValue"));

            if((overwrite.HasValue && overwrite.Value) || this.Request.Cookies["ThirdCookie"] == null)
                this.Response.Cookies.Add(new HttpCookie("ThirdCookie", "ThirdCookieValue"));

            return this.Content("Getting cookies");
        }

        #endregion
    }
}