using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using HansKindberg.Web.Simulation.Serialization.Extensions;

namespace HansKindberg.Web.Simulation.Serialization
{
    public class DefaultSerializableResolver : ISerializableResolver
    {
        #region Fields

        private const string _instanceSerializationInformationName = "Instance";
        private const string _instanceTypeSerializationInformationName = "Type";
        private static readonly IDictionary<Type, bool> _isSerializableCache = new Dictionary<Type, bool>();
        private const string _isSerializableSerializationInformationName = "IsSerializable";
        private static readonly object _lockObject = new object();

        #endregion

        #region Methods

        public virtual IEnumerable<FieldInfo> GetFields(Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            while(type != null)
            {
                foreach(FieldInfo field in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    yield return field;
                }

                type = type.BaseType;
            }
        }

        protected internal virtual IEnumerable<Type> GetGenericArgumentsRecursive(Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            foreach(Type genericArgument in type.GetGenericArguments())
            {
                yield return genericArgument;

                foreach(Type childGenericArgument in this.GetGenericArgumentsRecursive(genericArgument))
                {
                    yield return childGenericArgument;
                }
            }
        }

        public virtual TInstance GetInstance<TInstance>(SerializationInfo serializationInformation)
        {
            if(serializationInformation == null)
                throw new ArgumentNullException("serializationInformation");

            TInstance instance = default(TInstance);

            if(serializationInformation.Exists(_instanceSerializationInformationName))
                instance = (TInstance) serializationInformation.GetValue(_instanceSerializationInformationName, typeof(TInstance));

            return instance;
        }

        public virtual Type GetInstanceType(SerializationInfo serializationInformation)
        {
            if(serializationInformation == null)
                throw new ArgumentNullException("serializationInformation");

            Type type = null;

            if(serializationInformation.Exists(_instanceTypeSerializationInformationName))
                type = (Type) serializationInformation.GetValue(_instanceTypeSerializationInformationName, typeof(Type));

            return type;
        }

        public virtual bool GetIsSerializable(SerializationInfo serializationInformation)
        {
            if(serializationInformation == null)
                throw new ArgumentNullException("serializationInformation");

            bool isSerializable = false;

            if(serializationInformation.Exists(_isSerializableSerializationInformationName))
                isSerializable = serializationInformation.GetBoolean(_isSerializableSerializationInformationName);

            return isSerializable;
        }

        public virtual string GetSerializationInformationName(FieldInfo field)
        {
            if(field == null)
                throw new ArgumentNullException("field");

            if(field.DeclaringType == null)
                throw new ArgumentException("The field has no declaring type.", "field");

            return "Field:" + (this.IsSerializable(field.FieldType) ? "SerializableFieldType:" : string.Empty) + field.DeclaringType.FullName + ":" + field.Name;
        }

        public virtual bool IsSerializable(Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            bool isSerializable;

            if(!_isSerializableCache.TryGetValue(type, out isSerializable))
            {
                lock(_lockObject)
                {
                    if(!_isSerializableCache.TryGetValue(type, out isSerializable))
                    {
                        isSerializable = type.IsSerializable && this.GetGenericArgumentsRecursive(type).All(genericArgument => genericArgument.IsSerializable);
                        _isSerializableCache.Add(type, isSerializable);
                    }
                }
            }

            return isSerializable;
        }

        public virtual bool IsSerializable(object instance)
        {
            return instance == null || this.IsSerializable(instance.GetType());
        }

        public virtual void SetInstance(SerializationInfo serializationInformation, object instance)
        {
            if(serializationInformation == null)
                throw new ArgumentNullException("serializationInformation");

            serializationInformation.AddValue(_instanceSerializationInformationName, instance);
        }

        public virtual void SetInstanceType(SerializationInfo serializationInformation, Type type)
        {
            if(serializationInformation == null)
                throw new ArgumentNullException("serializationInformation");

            serializationInformation.AddValue(_instanceTypeSerializationInformationName, type);
        }

        public virtual void SetIsSerializable(SerializationInfo serializationInformation, bool serializable)
        {
            if(serializationInformation == null)
                throw new ArgumentNullException("serializationInformation");

            serializationInformation.AddValue(_isSerializableSerializationInformationName, serializable);
        }

        public virtual bool TryAddFieldValueToSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation)
        {
            if(instance == null)
                throw new ArgumentNullException("instance");

            if(field == null)
                throw new ArgumentNullException("field");

            if(serializationInformation == null)
                throw new ArgumentNullException("serializationInformation");

            if(this.IsSerializable(field.FieldType))
            {
                object fieldValue = field.GetValue(instance);

                if(this.IsSerializable(fieldValue))
                {
                    serializationInformation.AddValue(this.GetSerializationInformationName(field), field.GetValue(instance));
                    return true;
                }
            }

            return false;
        }

        public virtual bool TrySetFieldValueFromSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation)
        {
            if(instance == null)
                throw new ArgumentNullException("instance");

            if(field == null)
                throw new ArgumentNullException("field");

            if(serializationInformation == null)
                throw new ArgumentNullException("serializationInformation");

            if(this.IsSerializable(field.FieldType))
            {
                object fieldValue;

                if(serializationInformation.TryGetValue(this.GetSerializationInformationName(field), out fieldValue))
                {
                    field.SetValue(instance, fieldValue);
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}