using System;
using System.Text.RegularExpressions;

namespace HansKindberg.Web.Mvc.Simulation.Extensions
{
    public static class StringExtension
    {
        #region Methods

        public static string ExtractAntiForgeryToken(this string value)
        {
            if(value == null) throw new ArgumentNullException("value");

            Match match = Regex.Match(value, @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");
            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }

        #endregion
    }
}