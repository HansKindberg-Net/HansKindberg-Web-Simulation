using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HansKindberg.Web.Simulation.Serialization
{
    /// <summary>
    /// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
    /// </summary>
    [Serializable]
    public class Serializable : Serializable<object>
    {
        #region Constructors

        public Serializable(object instance, Type type) : base(instance, type) {}
        protected Serializable(SerializationInfo info, StreamingContext context) : base(info, context) {}

        #endregion
    }

    /// <summary>
    /// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
    /// </summary>
    [Serializable]
    public class Serializable<TInstance> : Serializable<TInstance, DefaultSerializableResolver>
    {
        #region Constructors

        public Serializable(TInstance instance) : base(instance) {}
        protected Serializable(TInstance instance, Type type) : base(instance, type) {}
        protected Serializable(SerializationInfo info, StreamingContext context) : base(info, context) {}

        #endregion
    }

    /// <summary>
    /// This code is originally from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
    /// </summary>
    [Serializable]
    public class Serializable<TInstance, TSerializableResolver> : ISerializable where TSerializableResolver : ISerializableResolver, new()
    {
        #region Fields

        private readonly TInstance _instance;
        private readonly ISerializableResolver _serializableResolver = new TSerializableResolver();
        private readonly Type _type;

        #endregion

        #region Constructors

        public Serializable(TInstance instance) : this(instance, typeof(TInstance)) {}

        public Serializable(TInstance instance, Type type)
        {
            if(Equals(instance, null))
                throw new ArgumentNullException("instance");

            if(type == null)
                throw new ArgumentNullException("type");

            this._instance = instance;
            this._type = type;
        }

        protected Serializable(SerializationInfo info, StreamingContext context)
        {
            if(info == null)
                throw new ArgumentNullException("info");

            if(this.SerializableResolver.GetIsSerializable(info))
            {
                this._instance = this.SerializableResolver.GetInstance<TInstance>(info);
                return;
            }

            Type type = this.SerializableResolver.GetInstanceType(info);
            this._instance = (TInstance) SerializableObjectFactory.Instance.CreateInstance(type);

            foreach(FieldInfo field in this.SerializableResolver.GetFields(type))
            {
                if(this.SerializableResolver.TrySetFieldValueFromSerializationInformation(this._instance, field, info))
                    continue;

                Serializable serializable = (Serializable) info.GetValue(this.SerializableResolver.GetSerializationInformationName(field), typeof(Serializable));
                field.SetValue(this._instance, serializable != null ? serializable.Instance : null);
            }
        }

        #endregion

        #region Properties

        public virtual TInstance Instance
        {
            get { return this._instance; }
        }

        protected internal ISerializableResolver SerializableResolver
        {
            get { return this._serializableResolver; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected internal virtual Type Type
        {
            get { return this._type; }
        }

        #endregion

        #region Methods

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if(info == null)
                throw new ArgumentNullException("info");

            if(this.SerializableResolver.IsSerializable(this.Type) && this.SerializableResolver.IsSerializable(this.Instance))
            {
                this.SerializableResolver.SetInstance(info, this.Instance);
                this.SerializableResolver.SetIsSerializable(info, true);
                return;
            }

            this.SerializableResolver.SetIsSerializable(info, false);
            this.SerializableResolver.SetInstanceType(info, this.Type);

            foreach(FieldInfo field in this.SerializableResolver.GetFields(this.Type))
            {
                if(this.SerializableResolver.TryAddFieldValueToSerializationInformation(this.Instance, field, info))
                    continue;

                Serializable serializable = null;
                object fieldValue = field.GetValue(this.Instance);

                if(fieldValue != null)
                    serializable = new Serializable(fieldValue, field.FieldType);

                info.AddValue(this.SerializableResolver.GetSerializationInformationName(field), serializable);
            }
        }

        #endregion
    }
}