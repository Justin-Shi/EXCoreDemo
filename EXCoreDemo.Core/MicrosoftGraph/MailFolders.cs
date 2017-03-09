using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXCoreDemo.Core.MicrosoftGraph
{
    /// <summary>
    /// Microsoft Graph API:
    ///     GET /me/mailFolders/{id}
    ///     GET /users/{id | userPrincipalName}/mailFolders/{id}
    /// </summary>
    public class MailFolders : MicrosoftGraphBase
    {
        #region Constructor
        public MailFolders(string _accessToken) : base(_accessToken)
        {

        }
        #endregion

        #region
        #endregion
    }
}
