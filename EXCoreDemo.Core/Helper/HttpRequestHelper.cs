using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EXCoreDemo.Core.Helper
{
    public class HttpRequestHelper
    {
        public static async Task<string> CallRestApiRequestAsync(string api, string token, string requestBody, HttpMethod method)
        {
            Func<HttpRequestMessage> requestCreator = () =>
            {
                HttpRequestMessage request = new HttpRequestMessage(method, api);
                request.Headers.Add("Accept", "application/json; odata.metadata=none");
                return request;
            };
            
            return await CallHttpRequestAsync(requestCreator, token, requestBody);
        }

        private static async Task<string> CallHttpRequestAsync(Func<HttpRequestMessage> requestCreator, string accessToken, string requestBody = "")
        {
            using (HttpClient client = new HttpClient())
            {
                // add propper instrumentation headers
                string clientRequestId = Guid.NewGuid().ToString();
                client.DefaultRequestHeaders.Add("client-request-id", clientRequestId);
                client.DefaultRequestHeaders.Add("return-client-request-id", "true");
                client.DefaultRequestHeaders.Add("UserAgent", "MatthiasLeibmannsAppOnlyAppSampleBeta/0.1");
                client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", accessToken));

                using (HttpRequestMessage request = requestCreator.Invoke())
                {
                    if (request.Method != HttpMethod.Get && request.Method != HttpMethod.Delete && !string.IsNullOrWhiteSpace(requestBody))
                    {
                        request.Content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
                    }

                    try
                    {
                        HttpResponseMessage httpResponse = await client.SendAsync(request);
                        if (httpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            return await httpResponse.Content.ReadAsStringAsync();
                        }

                        string error = $"\"error\" : \"{httpResponse.ReasonPhrase}\"";
                        if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            //httpResponse.Headers.Select(h => h.Key.Equals("x-ms-diagnostics")).First();
                            foreach (var header in httpResponse.Headers)
                            {
                                // x-ms-diagnostics contains details why request was unauthorized
                                if (header.Key.Equals("x-ms-diagnostics"))
                                {
                                    string e = string.Format("{0}", header.Value.ToArray());
                                    e = e.Replace("\"", "'");
                                    error = error + $", \"x-ms-diagnostics\" : \"{e}\"";;
                                }
                            }
                        }

                        return $"{{{error}}}";
                    }
                    catch (WebException webex)
                    {
                        HttpWebResponse httpWebResponse = webex.Response as HttpWebResponse;

                        if (httpWebResponse != null)
                        {
                            using (Stream serviceResponseStream = httpWebResponse.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(serviceResponseStream))
                                {
                                    JObject jsonResponse = JObject.Parse(reader.ReadToEnd());
                                    return jsonResponse.ToString();
                                }
                            }
                        }

                        string error = $"\"error\" : \"{webex.Message}\"";
                        return $"{{{error}}}";
                    }
                    catch (Exception ex)
                    {
                        string error = $"\"error\" : \"{ex.Message}\"";
                        return $"{{{error}}}";
                    }
                }
            }
        }
    }
}
