using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using Moq;

namespace HansKindberg.Web.Simulation.Serialization
{
    public class DefaultSerializableObjectFactory : IObjectFactory
    {
        #region Fields

        private static readonly IDictionary<Type, ConstructorInfo> _defaultConstructorCache = new Dictionary<Type, ConstructorInfo>
            {
                {typeof(HttpContext), typeof(HttpContext).GetConstructors(BindingFlags.Instance | BindingFlags.Public).First(constructorInfo => constructorInfo.GetParameters().Length == 1)},
                {typeof(HttpRequest), typeof(HttpRequest).GetConstructors(BindingFlags.Instance | BindingFlags.Public).First(constructorInfo => constructorInfo.GetParameters().Length == 3)}
            };

        private static readonly IDictionary<Type, IEnumerable<object>> _defaultConstructorParametersCache = new Dictionary<Type, IEnumerable<object>>
            {
                {typeof(HttpContext), new object[] {new SimpleWorkerRequest("/", AppDomain.CurrentDomain.BaseDirectory, "Default.html", string.Empty, Mock.Of<TextWriter>())}},
                {typeof(HttpRequest), new object[] {"Default.html", "http://localhost/Default.html", string.Empty}}
            };

        private static readonly object _lockObject = new object();

        #endregion

        #region Methods

        public virtual T CreateInstance<T>()
        {
            return (T) this.CreateInstance(typeof(T));
        }

        public virtual object CreateInstance(Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            try
            {
                if(type.IsAbstract || type.IsInterface)
                    return ((Mock) Activator.CreateInstance(typeof(Mock<>).MakeGenericType(new[] {type}))).Object;

                if(type == typeof(string))
                    return string.Empty;

                ConstructorInfo constructor = this.GetDefaultConstructor(type);

                return constructor.Invoke(this.GetConstructorParameters(constructor).ToArray());
            }
            catch(Exception exception)
            {
                throw new TargetInvocationException(string.Format(CultureInfo.InvariantCulture, "An object of type \"{0}\" could not be created.", type), exception);
            }
        }

        protected internal virtual IEnumerable<object> GetConstructorParameters(ConstructorInfo constructor)
        {
            if(constructor == null)
                throw new ArgumentNullException("constructor");

            IEnumerable<object> constructorParameters;

            if(!_defaultConstructorParametersCache.TryGetValue(constructor.DeclaringType, out constructorParameters))
                constructorParameters = constructor.GetParameters().Select(parameter => this.CreateInstance(parameter.ParameterType));

            return constructorParameters;
        }

        protected internal virtual ConstructorInfo GetDefaultConstructor(Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            ConstructorInfo defaultConstructor;

            if(!_defaultConstructorCache.TryGetValue(type, out defaultConstructor))
            {
                IEnumerable<ConstructorInfo> constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                ConstructorInfo parameterlessConstructor = constructors.FirstOrDefault(constructorInfo => constructorInfo.GetParameters().Length == 0);

                if(parameterlessConstructor != null)
                {
                    if(!_defaultConstructorCache.TryGetValue(type, out defaultConstructor))
                    {
                        lock(_lockObject)
                        {
                            defaultConstructor = parameterlessConstructor;
                            _defaultConstructorCache.Add(type, defaultConstructor);
                        }
                    }
                }
                else
                {
                    ConstructorInfo constructorWithLeastParameters = constructors.OrderBy(constructorInfo => constructorInfo.GetParameters().Length).First();

                    if(!_defaultConstructorCache.TryGetValue(type, out defaultConstructor))
                    {
                        lock(_lockObject)
                        {
                            defaultConstructor = constructorWithLeastParameters;
                            _defaultConstructorCache.Add(type, defaultConstructor);
                        }
                    }
                }
            }

            return defaultConstructor;
        }

        #endregion
    }
}