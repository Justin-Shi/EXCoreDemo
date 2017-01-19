using EXCoreDemo.Core.Helper;
using Microsoft.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Console;

namespace EXCoreDemo.Core.MicrosoftGraph
{
    /// <summary>
    /// Microsoft Graph API: 
    ///  GET /me/messages/{id}
    ///  GET /users/{id | userPrincipalName}/messages/{id}
    ///  GET /me/mailFolders/{id}/messages/{id}
    ///  GET /users/{id | userPrincipalName}/mailFolders/{id}/messages/{id}
    /// The Message model definied in: Microsoft.Graph
    /// </summary>
    public class Messages : MicrosoftGraphBase
    {
        #region Constructor
        public Messages(string _accessToken) : base(_accessToken)
        {
        }
        #endregion

        #region Methods

        #region message list
        /// <summary>
        /// Get a specified user's messages
        /// </summary>
        /// <param name="uid">The user Id or userPrincipalName.</param>
        /// <returns>The message list, null or empty when there is no messages.</returns>
        public async Task<List<Message>> GetMessagesByUserAsync(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException(nameof(uid));
            }

            string url = ConstructUrl(MICROSOFT_GRAPH_RESOURCE_URI, $"users/{uid}/messages", null);
            List<Message> messages = new List<Message>();
            string exception = string.Empty;

            try
            {
                string result = await HttpRequestHelper.CallRestApiRequestAsync(url, AccessToken, null, HttpMethod.Get);
                var data = JObject.Parse(result).SelectToken("value");

                if (data != null)
                {
                    messages = JsonConvert.DeserializeObject<List<Message>>(data.ToString());
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

            return messages;
        }

        /// <summary>
        /// Get a specified user's messages under a specified folder
        /// </summary>
        /// <param name="uid">The user Id or userPrincipalName.</param>
        /// <param name="folderId">The mail folder Id.</param>
        /// <returns>The message list, null or empty when there is no messages.</returns>
        public async Task<List<Message>> GetMessagesUnderFolderByUserAsync(string uid, string folderId)
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException(nameof(uid));
            }
            if (string.IsNullOrEmpty(folderId))
            {
                throw new ArgumentNullException(folderId);
            }

            string url = ConstructUrl(MICROSOFT_GRAPH_RESOURCE_URI, $"users/{uid}/mailFolders/{folderId}/messages", null);
            List<Message> messages = new List<Message>();
            string exception = string.Empty;

            try
            {
                string result = await HttpRequestHelper.CallRestApiRequestAsync(url, AccessToken, null, HttpMethod.Get);
                var data = JObject.Parse(result).SelectToken("value");

                if (data != null)
                {
                    messages = JsonConvert.DeserializeObject<List<Message>>(data.ToString());
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

            return messages;
        }
        #endregion

        #region message
        /// <summary>
        /// Get a specified user's specified messages
        /// </summary>
        /// <param name="uid">The user Id or userPrincipalName.</param>
        /// <param name="messageId">The message Id.</param>
        /// <returns>The message, null when the message is not existed.</returns>
        public async Task<Message> GetMessageByUserAsync(string uid, string messageId)
        {
            if (!string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException(nameof(uid));
            }
            if (!string.IsNullOrEmpty(messageId))
            {
                throw new ArgumentNullException(nameof(messageId));
            }


            string url = ConstructUrl(MICROSOFT_GRAPH_RESOURCE_URI, $"users/{uid}/messages/{messageId}", null);
            Message message = null;
            string exception = string.Empty;

            try
            {
                string result = await HttpRequestHelper.CallRestApiRequestAsync(url, AccessToken, null, HttpMethod.Get);
                var data = JObject.Parse(result);

                if (data != null)
                {
                    message = JsonConvert.DeserializeObject<Message>(data.ToString());
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

            return message;
        }

        /// <summary>
        /// Get a specified user's message under a specified folder
        /// </summary>
        /// <param name="uid">The user Id or userPrincipalName.</param>
        /// <param name="folderId">The mail folder Id.</param>
        /// <param name="messageId">The message Id.</param>
        /// <returns>The message, null when the messages is not existed.</returns>
        public async Task<Message> GetMessageUnderFolderByUserAsync(string uid, string folderId, string messageId)
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException(nameof(uid));
            }
            if (string.IsNullOrEmpty(folderId))
            {
                throw new ArgumentNullException(folderId);
            }
            if (string.IsNullOrEmpty(messageId))
            {
                throw new ArgumentNullException(nameof(messageId));
            }

            string url = ConstructUrl(MICROSOFT_GRAPH_RESOURCE_URI, $"users/{uid}/mailFolders/{folderId}/messages/{messageId}", null);
            Message message = null;
            string exception = string.Empty;

            try
            {
                string result = await HttpRequestHelper.CallRestApiRequestAsync(url, AccessToken, null, HttpMethod.Get);
                var data = JObject.Parse(result);

                if (data != null)
                {
                    message = JsonConvert.DeserializeObject<Message>(data.ToString());
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

            return message;
        }
        #endregion

        public static string FormatListMessage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return $"Subject:           {message.Subject ?? string.Empty}{Environment.NewLine}"
                 + $"From:              {message.From?.EmailAddress.Address}{Environment.NewLine}"
                 + $"To:                {string.Join("; ", message.ToRecipients?.Select(x=>x.EmailAddress.Address))}{Environment.NewLine}"
                 + $"Id:                {message.Id}{Environment.NewLine}";
        }
        #endregion
    }
}
