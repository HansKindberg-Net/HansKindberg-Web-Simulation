using System;
using System.Runtime.Serialization;

namespace HansKindberg.Web.Simulation.Serialization.Extensions
{
    public static class SerializationInfoExtension
    {
        #region Methods

        public static bool Exists(this SerializationInfo serializationInfo, string name)
        {
            if(serializationInfo == null)
                throw new ArgumentNullException("serializationInfo");

            if(name == null)
                return false;

            foreach(SerializationEntry entry in serializationInfo)
            {
                if(entry.Name.Equals(name))
                    return true;
            }

            return false;
        }

        public static bool TryGetValue<T>(this SerializationInfo serializationInfo, string name, out T value)
        {
            if(serializationInfo == null)
                throw new ArgumentNullException("serializationInfo");

            value = default(T);

            if(!serializationInfo.Exists(name))
                return false;

            value = (T) serializationInfo.GetValue(name, typeof(T));

            return true;
        }

        #endregion
    }
}