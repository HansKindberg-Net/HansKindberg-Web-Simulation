using System.Diagnostics.CodeAnalysis;
using WebFormsMvp;

namespace HansKindberg.Web.Simulation.Application.Business.Web.Mvp.UI
{
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
    public interface ISiteView : IView {}

    public interface ISiteView<TModel> : ISiteView where TModel : class
    {
        #region Properties

        TModel Model { get; set; }

        #endregion
    }
}