using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace HansKindberg.Web.Simulation.Serialization
{
    public class SerializableDelegateResolver<TDelegate> : DefaultSerializableResolver
    {
        #region Fields

        private readonly ITypeValidator _typeValidator = new TypeValidator();

        #endregion

        #region Constructors

        public SerializableDelegateResolver()
        {
            this._typeValidator.ValidateThatTheTypeIsADelegate(typeof(TDelegate));
        }

        #endregion

        #region Methods

        public override bool TryAddFieldValueToSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation)
        {
            if(instance == null)
                throw new ArgumentNullException("instance");

            if(field == null)
                throw new ArgumentNullException("field");

            if(serializationInformation == null)
                throw new ArgumentNullException("serializationInformation");

            if(typeof(Delegate).IsAssignableFrom(field.FieldType))
            {
                serializationInformation.AddValue(this.GetSerializationInformationName(field), new SerializableDelegate<TDelegate>((TDelegate) field.GetValue(instance)));
                return true;
            }

            return base.TryAddFieldValueToSerializationInformation(instance, field, serializationInformation);
        }

        public override bool TrySetFieldValueFromSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation)
        {
            if(instance == null)
                throw new ArgumentNullException("instance");

            if(field == null)
                throw new ArgumentNullException("field");

            if(serializationInformation == null)
                throw new ArgumentNullException("serializationInformation");

            if(typeof(Delegate).IsAssignableFrom(field.FieldType))
            {
                field.SetValue(instance, ((SerializableDelegate<TDelegate>) serializationInformation.GetValue(this.GetSerializationInformationName(field), typeof(SerializableDelegate<TDelegate>))).DelegateInstance);
                return true;
            }

            return base.TrySetFieldValueFromSerializationInformation(instance, field, serializationInformation);
        }

        #endregion
    }
}