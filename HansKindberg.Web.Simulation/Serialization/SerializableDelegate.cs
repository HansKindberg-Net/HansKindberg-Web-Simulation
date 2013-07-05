using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HansKindberg.Web.Simulation.Serialization
{
    /// <summary>
    /// Makes delegates serializable where possible.
    /// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class SerializableDelegate<TDelegate> : ISerializable
    {
        #region Fields

        private const string _anonymousDelegateSerializationInformationName = "AnonymousDelegate";
        private readonly TDelegate _delegateInstance;
        private const string _methodSerializationInformationName = "Method";
        private readonly ISerializableResolver _serializableResolver = new DefaultSerializableResolver();
        private readonly ITypeValidator _typeValidator = new TypeValidator();

        #endregion

        #region Constructors

        public SerializableDelegate(TDelegate delegateInstance)
        {
            this._typeValidator.ValidateThatTheTypeIsADelegate(typeof(TDelegate));

            if(Equals(delegateInstance, null))
                throw new ArgumentNullException("delegateInstance");

            this._delegateInstance = delegateInstance;
        }

        protected SerializableDelegate(SerializationInfo info, StreamingContext context)
        {
            if(info == null)
                throw new ArgumentNullException("info");

            if(this.SerializableResolver.GetIsSerializable(info))
            {
                this._delegateInstance = this.SerializableResolver.GetInstance<TDelegate>(info);
                return;
            }

            Type type = this.SerializableResolver.GetInstanceType(info);
            MethodInfo methodInfo = (MethodInfo) info.GetValue(_methodSerializationInformationName, typeof(MethodInfo));
            Serializable<object, SerializableDelegateResolver<TDelegate>> anonymousDelegate = (Serializable<object, SerializableDelegateResolver<TDelegate>>) info.GetValue(_anonymousDelegateSerializationInformationName, typeof(Serializable<object, SerializableDelegateResolver<TDelegate>>));
            this._delegateInstance = (TDelegate) (object) System.Delegate.CreateDelegate(type, anonymousDelegate.Instance, methodInfo);
        }

        #endregion

        #region Properties

        public virtual TDelegate DelegateInstance
        {
            get { return this._delegateInstance; }
        }

        protected internal ISerializableResolver SerializableResolver
        {
            get { return this._serializableResolver; }
        }

        #endregion

        #region Methods

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if(info == null)
                throw new ArgumentNullException("info");

            Delegate unTypedDelegate = (Delegate) (object) this.DelegateInstance;

            if(this.IsSerializableDelegate(unTypedDelegate))
            {
                this.SerializableResolver.SetIsSerializable(info, true);
                this.SerializableResolver.SetInstance(info, this.DelegateInstance);
                return;
            }

            this.SerializableResolver.SetIsSerializable(info, false);
            this.SerializableResolver.SetInstanceType(info, this.DelegateInstance.GetType());
            info.AddValue(_methodSerializationInformationName, unTypedDelegate.Method);
            info.AddValue(_anonymousDelegateSerializationInformationName, new Serializable<object, SerializableDelegateResolver<TDelegate>>(unTypedDelegate.Target, unTypedDelegate.Method.DeclaringType));
        }

        protected internal virtual bool IsSerializableDelegate(Delegate unTypedDelegate)
        {
            if(unTypedDelegate == null)
                throw new ArgumentNullException("unTypedDelegate");

            if(unTypedDelegate.Target == null)
                return true;

            if(unTypedDelegate.Method == null)
                return false;

            if(unTypedDelegate.Method.DeclaringType == null)
                return false;

            return unTypedDelegate.Method.DeclaringType.GetCustomAttributes(typeof(SerializableAttribute), false).Length > 0;
        }

        #endregion
    }
}