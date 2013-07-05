using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace HansKindberg.Web.Simulation.Application.Business.Web.Mvp.UI
{
    public class MvpPage : WebFormsMvp.Web.MvpPage
    {
        #region Fields

        private readonly HtmlHead _htmlHead = new HtmlHead();
        private Action<HtmlHead> _setHeaderDelegate;
        private static readonly MethodInfo _setHeaderMethodInfo = typeof(Page).GetMethod("SetHeader", BindingFlags.Instance | BindingFlags.NonPublic);

        #endregion

        #region Constructors

        public MvpPage()
        {
            this.AutoDataBind = false;
        }

        #endregion

        #region Methods

        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        [SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly")]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public override sealed void Dispose()
        {
            this.Dispose(true);

            base.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
                this._htmlHead.Dispose();
        }

        protected internal virtual void SetHtmlHead(HtmlHead htmlHead)
        {
            if(htmlHead == null)
                throw new ArgumentNullException("htmlHead");

            if(this._setHeaderDelegate == null)
                this._setHeaderDelegate = (Action<HtmlHead>) Delegate.CreateDelegate(typeof(Action<HtmlHead>), this, _setHeaderMethodInfo);

            this._setHeaderDelegate(htmlHead);
        }

        #endregion

        #region Eventhandlers

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            this.SetHtmlHead(this._htmlHead);
        }

        #endregion
    }

    public class MvpPage<TModel> : MvpPage, ISiteView<TModel> where TModel : class
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