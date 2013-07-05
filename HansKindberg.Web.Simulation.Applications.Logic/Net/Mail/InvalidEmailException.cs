using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace HansKindberg.Web.Simulation.Applications.Logic.Net.Mail
{
    [Serializable]
    public class InvalidEmailException : Exception
    {
        #region Fields

        private const string _invalidEmailAddressExceptionMessageFormat = "The email-address \"{0}\" is invalid.";
        private const string _nullEmailAddressExceptionMessageFormat = "The email-address can not be null.";

        #endregion

        #region Constructors

        public InvalidEmailException() {}
        public InvalidEmailException(string emailAddress) : this(emailAddress, null) {}
        public InvalidEmailException(string emailAddress, Exception innerException) : base(CreateMessage(emailAddress), innerException) {}
        protected InvalidEmailException(SerializationInfo info, StreamingContext context) : base(info, context) {}

        #endregion

        #region Methods

        protected static string CreateMessage(string emailAddress)
        {
            return emailAddress == null ? _nullEmailAddressExceptionMessageFormat : string.Format(CultureInfo.InvariantCulture, _invalidEmailAddressExceptionMessageFormat, emailAddress);
        }

        #endregion
    }
}