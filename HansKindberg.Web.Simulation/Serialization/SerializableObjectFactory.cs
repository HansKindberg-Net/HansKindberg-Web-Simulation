using System;

namespace HansKindberg.Web.Simulation.Serialization
{
    public static class SerializableObjectFactory
    {
        #region Fields

        private static IObjectFactory _instance;

        #endregion

        #region Properties

        public static IObjectFactory Instance
        {
            get { return _instance ?? (_instance = new DefaultSerializableObjectFactory()); }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                _instance = value;
            }
        }

        #endregion
    }
}