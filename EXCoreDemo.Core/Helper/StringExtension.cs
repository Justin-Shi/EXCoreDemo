using System.Text.RegularExpressions;

namespace EXCoreDemo.Core.Helper
{
    public static class StringExtension
    {
        public static bool IsValidEmailAddress(this string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Copied from: http://www.regular-expressions.info/email.html
            string emailRegexPattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";
            Regex rgx = new Regex(emailRegexPattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(email);
            if (matches.Count == 0)
                return false;

            return true;
        }
    }
}
