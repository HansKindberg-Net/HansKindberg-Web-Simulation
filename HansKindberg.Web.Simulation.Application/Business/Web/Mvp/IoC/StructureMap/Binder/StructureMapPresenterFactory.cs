using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StructureMap;
using StructureMap.Pipeline;
using WebFormsMvp;
using WebFormsMvp.Binder;

namespace HansKindberg.Web.Simulation.Application.Business.Web.Mvp.IoC.StructureMap.Binder
{
    public class StructureMapPresenterFactory : IPresenterFactory, IDisposable
    {
        #region Fields

        private readonly IContainer _container;
        private static readonly string _genericIPresenterFriendlyName = typeof(IPresenter).FullName + "<TView>";
        private readonly IDictionary<Type, Type> _presenterTypeToViewTypeMappings = new Dictionary<Type, Type>();
        private readonly object _presenterTypeToViewTypeMappingsLockObject = new object();

        #endregion

        #region Constructors

        public StructureMapPresenterFactory(IContainer container)
        {
            if(container == null)
                throw new ArgumentNullException("container");

            this._container = container;
        }

        #endregion

        #region Methods

        public virtual IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            Type abstractViewType = this.GetAbstractViewType(presenterType);

            if(viewType == null)
                throw new ArgumentNullException("viewType");

            if(!typeof(IView).IsAssignableFrom(viewType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The view-type \"{0}\" must implement \"{1}\".", viewType.FullName, typeof(IView).FullName), "viewType");

            if(viewInstance == null)
                throw new ArgumentNullException("viewInstance");

            ExplicitArguments explicitArguments = new ExplicitArguments();
            explicitArguments.Set(abstractViewType, viewInstance);

            IPresenter presenter = (IPresenter) this._container.GetInstance(presenterType, explicitArguments);

            if(presenter == null)
                throw new StructureMapException(202, new object[] {presenterType});

            return presenter;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing && this._container != null)
                this._container.Dispose();
        }

        protected internal virtual Type GetAbstractViewType(Type presenterType)
        {
            if(presenterType == null)
                throw new ArgumentNullException("presenterType");

            lock(this._presenterTypeToViewTypeMappingsLockObject)
            {
                if(!this._presenterTypeToViewTypeMappings.ContainsKey(presenterType))
                {
                    try
                    {
                        // ReSharper disable PossibleNullReferenceException
                        Type viewType = presenterType
                            .GetInterfaces()
                            .SingleOrDefault(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IPresenter<>))
                            .GetGenericArguments()[0];
                        // ReSharper restore PossibleNullReferenceException

                        if(viewType == null)
                            throw new ArgumentNullException("presenterType", "The view-type is null.");

                        this._presenterTypeToViewTypeMappings.Add(presenterType, viewType);

                        return viewType;
                    }
                    catch(Exception exception)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Could not resolve the view-type for presenter-type \"{0}\". The presenter-type must implement \"{1}\".", presenterType.FullName, _genericIPresenterFriendlyName), "presenterType", exception);
                    }
                }

                return this._presenterTypeToViewTypeMappings[presenterType];
            }
        }

        public virtual void Release(IPresenter presenter)
        {
            this._container.EjectAllInstancesOf<IPresenter>();

            IDisposable disposablePresenter = presenter as IDisposable;

            if(disposablePresenter != null)
                disposablePresenter.Dispose();
        }

        #endregion
    }
}