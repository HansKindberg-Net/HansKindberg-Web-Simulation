using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace HansKindberg.Web.Simulation.Serialization
{
    public interface ISerializableResolver
    {
        #region Methods

        IEnumerable<FieldInfo> GetFields(Type type);
        TInstance GetInstance<TInstance>(SerializationInfo serializationInformation);
        Type GetInstanceType(SerializationInfo serializationInformation);
        bool GetIsSerializable(SerializationInfo serializationInformation);
        string GetSerializationInformationName(FieldInfo field);
        bool IsSerializable(Type type);
        bool IsSerializable(object instance);
        void SetInstance(SerializationInfo serializationInformation, object instance);
        void SetInstanceType(SerializationInfo serializationInformation, Type type);
        void SetIsSerializable(SerializationInfo serializationInformation, bool serializable);
        bool TryAddFieldValueToSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation);
        bool TrySetFieldValueFromSerializationInformation(object instance, FieldInfo field, SerializationInfo serializationInformation);

        #endregion
    }
}