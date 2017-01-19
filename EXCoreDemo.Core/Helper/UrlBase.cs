using System;
using System.Collections.Generic;
using System.Text;

namespace EXCoreDemo.Core.Helper
{
    public abstract class UrlBase
    {
        protected virtual string ConstructUrl(string urlPrefix, string urlSuffix, IDictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(urlPrefix))
            {
                throw new ArgumentNullException(nameof(urlPrefix));
            }

            const char UTL_SPLIT_CHARACTER = '/';
            const char QUESTION_MARK = '?';
            const char PARAMETER_CONTACT_CHARACTER = '&';
            string url = urlPrefix.TrimEnd(new char[] { UTL_SPLIT_CHARACTER });

            if (!string.IsNullOrEmpty(urlSuffix))
            {
                url = $"{url}{UTL_SPLIT_CHARACTER}{urlSuffix}";
            }

            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder parameterString = new StringBuilder(QUESTION_MARK.ToString());
                foreach (var pair in parameters)
                {
                    parameterString.Append($"{pair.Key}={pair.Value}{PARAMETER_CONTACT_CHARACTER}");
                }

                url = $"{url.Trim()}{parameterString.ToString().TrimEnd(new char[] { PARAMETER_CONTACT_CHARACTER })}";
            }

            return url;
        }
    }
}
