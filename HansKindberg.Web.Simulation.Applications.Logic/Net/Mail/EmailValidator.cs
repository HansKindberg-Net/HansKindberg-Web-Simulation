using System.Text.RegularExpressions;

namespace HansKindberg.Web.Simulation.Applications.Logic.Net.Mail
{
    public class EmailValidator : IEmailValidator
    {
        #region Fields

        private static readonly Regex _validateEmailRegex = new Regex
            (
            "^((?>[a-zA-Z\\d!#$%&'*+\\-/=?^_`{|}~]+\\x20*|\"((?=[\\x01-\\x7f])[^\"\\\\]|\\\\[\\x01-\\x7f])*\"\\x20*)*(?<angle><))?((?!\\.)(?>\\.?[a-zA-Z\\d!#$%&'*+\\-/=?^_`{|}~]+)+|\"((?=[\\x01-\\x7f])[^\"\\\\]|\\\\[\\x01-\\x7f])*\")@(((?!-)[a-zA-Z\\d\\-]+(?<!-)\\.)+[a-zA-Z]{2,}|\\[(((?(?<!\\[)\\.)(25[0-5]|2[0-4]\\d|[01]?\\d?\\d)){4}|[a-zA-Z\\d\\-]*[a-zA-Z\\d]:((?=[\\x01-\\x7f])[^\\\\\\[\\]]|\\\\[\\x01-\\x7f])+)\\])(?(angle)>)$",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase
            );

        #endregion

        #region Methods

        public virtual bool IsValidEmailAddress(string emailAddress)
        {
            return emailAddress != null && _validateEmailRegex.IsMatch(emailAddress);
        }

        public virtual void ValidateEmailAddress(string emailAddress)
        {
            if(this.IsValidEmailAddress(emailAddress))
                throw new InvalidEmailException(emailAddress);
        }

        #endregion
    }
}