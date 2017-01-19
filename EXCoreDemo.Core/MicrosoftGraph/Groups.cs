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
    public class Groups : MicrosoftGraphBase
    {
        private const string URL_SUFFIX = "groups";

        #region Constructor
        public Groups(string _accessToken) : base(_accessToken)
        {
        }
        #endregion

        #region methods

        #region Group List
        public async Task<List<Group>> GetAllGroupsAsync()
        {
            string url = ConstructUrl(MICROSOFT_GRAPH_RESOURCE_URI, URL_SUFFIX, null);
            List<Group> groups = new List<Group>();
            string exception = string.Empty;

            try
            {
                string result = await HttpRequestHelper.CallRestApiRequestAsync(url, AccessToken, null, HttpMethod.Get);
                var data = JObject.Parse(result).SelectToken("value");

                if (data != null)
                {
                    groups = JsonConvert.DeserializeObject<List<Group>>(data.ToString());
                }
                else
                {
                    exception = result;
                    WriteLine(exception);
                }
            }
            catch (Exception e)
            {
                exception = $"Message: {e.Message} StackTrace: {e.StackTrace}";
                WriteLine(exception);
            }

            return groups;
        }
        #endregion

        #region Group
        /// <summary>
        /// Get a specified group by the group id.
        /// </summary>
        /// <param name="id">The group Id.</param>
        /// <returns>The specified group. Null if it doesn't existed.</returns>
        public async Task<Group> GetUserAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            string url = ConstructUrl(MICROSOFT_GRAPH_RESOURCE_URI, $"{URL_SUFFIX}/{id}", null);
            Group group = null;
            string exception = string.Empty;

            try
            {
                string result = await HttpRequestHelper.CallRestApiRequestAsync(url, AccessToken, null, HttpMethod.Get);
                var data = JObject.Parse(result);

                if (data != null)
                {
                    group = JsonConvert.DeserializeObject<Group>(data.ToString());
                }
                else
                {
                    exception = result;
                    WriteLine(exception);
                }
            }
            catch (Exception e)
            {
                exception = $"Message: {e.Message} StackTrace: {e.StackTrace}";
                WriteLine(exception);
            }

            return group;
        }
        #endregion

        public static string FormatListGroup(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return $"DisplayName:       {group.DisplayName}{Environment.NewLine}"
                 + $"Id:                {group.Id}{Environment.NewLine}"
                 + $"MailEnabled:       {group.MailEnabled}{Environment.NewLine}"
                 + $"Description:       {group.Description}{Environment.NewLine}";
        }
        #endregion
    }
}
