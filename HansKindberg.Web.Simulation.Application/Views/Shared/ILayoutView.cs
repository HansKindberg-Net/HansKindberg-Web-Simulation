using System.Web.UI;
using HansKindberg.Web.Simulation.Application.Business.Web.Mvp.UI;
using HansKindberg.Web.Simulation.Application.Models;
using WebFormsMvp;

namespace HansKindberg.Web.Simulation.Application.Views.Shared
{
    public interface ILayoutView : IView
    {
        #region Properties

        Control HeadControl { get; }
        Control HeadingControl { get; }
        LayoutModel Model { get; set; }
        Control NavigationControl { get; }
        IPageView PageView { get; }

        #endregion
    }
}