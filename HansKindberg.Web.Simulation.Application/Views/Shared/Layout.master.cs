using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web.UI;
using HansKindberg.Web.Simulation.Application.Business.Web.Mvp.UI;
using HansKindberg.Web.Simulation.Application.Models;

namespace HansKindberg.Web.Simulation.Application.Views.Shared
{
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public partial class Layout : MvpMasterPage, ILayoutView
    {
        #region Fields

        private LayoutModel _model;
        private IPageView _pageView;

        #endregion

        #region Properties

        public virtual Control HeadControl
        {
            get { return this.headPlaceHolder; }
        }

        public virtual Control HeadingControl
        {
            get { return this.headingPlaceHolder; }
        }

        public virtual LayoutModel Model
        {
            get
            {
                if(this._model == null)
                    throw new InvalidOperationException("You must set the model of the view in your presenter.");

                return this._model;
            }
            set { this._model = value; }
        }

        public virtual Control NavigationControl
        {
            get { return this.navigationPlaceHolder; }
        }

        public virtual IPageView PageView
        {
            get
            {
                if(this._pageView == null)
                {
                    this._pageView = this.Page as IPageView;

                    if(this._pageView == null)
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Every page using \"{0}\" as master-page must inherit from \"{1}\".", typeof(Layout).FullName, typeof(IPageView).FullName));
                }

                return this._pageView;
            }
        }

        #endregion
    }
}