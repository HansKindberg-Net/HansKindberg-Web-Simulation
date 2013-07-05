using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HansKindberg.Web.Simulation.Applications.Logic.Extensions
{
    public static class StringExtension
    {
        #region Methods

        public static bool Like(this string value, string pattern)
        {
            return value.Like(pattern, '*');
        }

        public static bool Like(this string value, string pattern, char wildcard)
        {
            return value.Like(pattern, wildcard, true);
        }

        public static bool Like(this string value, string pattern, bool caseInsensitive)
        {
            return value.Like(pattern, '*', caseInsensitive);
        }

        public static bool Like(this string value, string pattern, char wildcard, bool caseInsensitive)
        {
            if(value == null)
                throw new ArgumentNullException("value");

            if(pattern == null)
                throw new ArgumentNullException("pattern");

            RegexOptions regexOptions = RegexOptions.Compiled;

            if(caseInsensitive)
                regexOptions |= RegexOptions.IgnoreCase;

            string regexPattern = pattern.Replace(wildcard.ToString(CultureInfo.InvariantCulture), "*");
            regexPattern = "^" + Regex.Escape(regexPattern).Replace("\\*", ".*") + "$";

            return Regex.IsMatch(value, regexPattern, regexOptions);
        }

        #endregion
    }
}