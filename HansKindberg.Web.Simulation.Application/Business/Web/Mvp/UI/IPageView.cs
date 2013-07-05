namespace HansKindberg.Web.Simulation.Application.Business.Web.Mvp.UI
{
    public interface IPageView : ISiteView
    {
        #region Properties

        string Title { get; }

        #endregion
    }

    public interface IPageView<TModel> : IPageView, ISiteView<TModel> where TModel : class {}
}