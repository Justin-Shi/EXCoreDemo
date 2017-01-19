using EXCoreDemo.Core.Helper;
using Microsoft.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Console;

namespace EXCoreDemo.Core.MicrosoftGraph
{
    /// <summary>
    /// Microsoft Graph API: https://graph.microsoft.com/v1.0/users
    /// The User model definied in: Microsoft.Graph
    /// </summary>
    public class Users : MicrosoftGraphBase
    {
        private const string URL_SUFFIX = "users";

        #region Constructor
        public Users(string _accessToken) : base(_accessToken)
        {
        }
        #endregion

        #region methods

        #region User List
        public async Task<List<User>> GetAllUsersAsync()
        {
            string url = ConstructUrl(MICROSOFT_GRAPH_RESOURCE_URI, URL_SUFFIX, null);
            List<User> users = new List<User>();
            string exception = string.Empty;

            try
            {
                string result = await HttpRequestHelper.CallRestApiRequestAsync(url, AccessToken, null, HttpMethod.Get);
                var data = JObject.Parse(result).SelectToken("value");

                if (data != null)
                {
                    users = JsonConvert.DeserializeObject<List<User>>(data.ToString());
                }
                else
                {
                    exception = result;
                    WriteLine(exception);
                }
            }
            catch(Exception e)
            {
                exception = $"Message: {e.Message} StackTrace: {e.StackTrace}";
                WriteLine(exception);
            }

            return users;
        }
        #endregion

        #region User
        /// <summary>
        /// Get a specified user by the uid. The uid can be: Id, userPrincipalName
        /// </summary>
        /// <param name="uid">The user Id or userPrincipalName</param>
        /// <returns>The specified user. Null if it doesn't existed.</returns>
        public async Task<User> GetUserAsync(string uid)
        {
            if (!string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException(nameof(uid));
            }

            string url = ConstructUrl(MICROSOFT_GRAPH_RESOURCE_URI, $"{URL_SUFFIX}/{uid}", null);
            User user = null;
            string exception = string.Empty;

            try
            {
                string result = await HttpRequestHelper.CallRestApiRequestAsync(url, AccessToken, null, HttpMethod.Get);
                var data = JObject.Parse(result);

                if (data != null)
                {
                    user = JsonConvert.DeserializeObject<User>(data.ToString());
                }
                else
                {
                    exception = result;
                    WriteLine(exception);
                }
            }
            catch(Exception e)
            {
                exception = $"Message: {e.Message} StackTrace: {e.StackTrace}";
                WriteLine(exception);
            }

            return user;
        }
        #endregion

        public static string FormatListUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return $"DisplayName:       {user.DisplayName}{Environment.NewLine}" 
                 + $"Id:                {user.Id}{Environment.NewLine}"
                 + $"GivenName:         {user.GivenName}{Environment.NewLine}"
                 + $"Mail:              {user.Mail}{Environment.NewLine}"
                 + $"UserPrincipalName: {user.UserPrincipalName}{Environment.NewLine}";
        }
        #endregion
    }
}
