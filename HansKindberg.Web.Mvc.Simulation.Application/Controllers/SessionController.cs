using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace HansKindberg.Web.Mvc.Simulation.Application.Controllers
{
    public class SessionController : Controller
    {
        #region Methods

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public virtual ActionResult GetSessions()
        {
            if(this.Session["FirstSession"] == null)
                this.Session["FirstSession"] = string.Empty;

            if(this.Session["SecondSession"] == null)
                this.Session["SecondSession"] = 10;

            if(this.Session["ThirdSession"] == null)
                this.Session["ThirdSession"] = true;

            this.Session["IncrementSession"] = ((int?) (this.Session["IncrementSession"] ?? 0)) + 1;

            return this.Content("Getting sessions");
        }

        #endregion
    }
}