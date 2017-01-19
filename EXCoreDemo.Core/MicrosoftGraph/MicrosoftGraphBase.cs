using System;
using EXCoreDemo.Core.Helper;

namespace EXCoreDemo.Core.MicrosoftGraph
{
    public abstract class MicrosoftGraphBase : UrlBase
    {
        protected const string MICROSOFT_GRAPH_RESOURCE_URI = "https://graph.microsoft.com/v1.0";

        #region Fields
        private string accessToken;
        #endregion

        #region Properties
        public string AccessToken
        {
            get
            {
                return accessToken;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(accessToken));
                }

                accessToken = value;
            }
        }
        #endregion

        #region Constructor
        public MicrosoftGraphBase(string _accessToken)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            accessToken = _accessToken;
        }
        #endregion
    }
}
