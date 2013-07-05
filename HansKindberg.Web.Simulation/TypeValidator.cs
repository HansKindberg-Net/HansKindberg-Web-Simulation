using System;
using System.Globalization;

namespace HansKindberg.Web.Simulation
{
    public class TypeValidator : ITypeValidator
    {
        #region Methods

        public virtual void ValidateThatTheTypeIsADelegate(Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if(!typeof(Delegate).IsAssignableFrom(type))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The type \"{0}\" must be a delegate ({1}).", type.FullName, typeof(Delegate)), "type");
        }

        #endregion
    }
}