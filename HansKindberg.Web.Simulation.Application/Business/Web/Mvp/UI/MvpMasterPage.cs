using System;
using System.Web.UI;
using WebFormsMvp.Web;

namespace HansKindberg.Web.Simulation.Application.Business.Web.Mvp.UI
{
    public class MvpMasterPage : MasterPage, ISiteView
    {
        #region Properties

        public virtual bool ThrowExceptionIfNoPresenterBound
        {
            get { return true; }
        }

        #endregion

        #region Methods

        protected override void OnInit(EventArgs e)
        {
            PageViewHost.Register(this, this.Context, false);
            base.OnInit(e);
        }

        #endregion
    }

    public class MvpMasterPage<TModel> : MvpMasterPage, ISiteView<TModel> where TModel : class, new()
    {
        #region Fields

        private TModel _model;

        #endregion

        #region Properties

        public virtual TModel Model
        {
            get
            {
                if(this._model == null)
                    throw new InvalidOperationException("You must set the model of the view in the presenter.");

                return this._model;
            }
            set { this._model = value; }
        }

        #endregion
    }
}