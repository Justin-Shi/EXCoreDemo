using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace EXCoreDemo.AccessToken
{
    public enum AuthenticationType
    {
        User,
        WebApp,
        NativeApp
    }

    public enum AuthenticationMethod
    {
        Cert = 0,
        Password = 1
    }

    public enum ResourceType
    {
        AzureGraphApi = 0,
        MicrosoftGraphApi = 1,
        Office365Api = 2
    }

    internal class AccessTokenHelper
    {
        /// <summary>
        /// Get Token for Web Application.
        /// </summary>
        /// <returns>Token for web application.</returns>
        public static async Task<string> GetTokenForWebApplication(string resource, AuthenticationMethod auMethod)
        {
            string tokenForWebApplication = string.Empty;

            AuthenticationContext authenticationContext = new AuthenticationContext(WebAppConstants.AUTH_STRING, false);

            switch (auMethod)
            {
                case AuthenticationMethod.Password:
                    ClientCredential clientCred = new ClientCredential(WebAppConstants.CLIENT_ID,
                        WebAppConstants.CLIENT_SECRET);
                    AuthenticationResult authenticationResult =
                        await authenticationContext.AcquireTokenAsync(resource,
                            clientCred);
                    tokenForWebApplication = authenticationResult.AccessToken;
                    break;

                case AuthenticationMethod.Cert:
                    var assembly = Assembly.GetExecutingAssembly();
                    var certName = "EXCoreDemo.AccessToken.Resources.EXCoreDemoWeb.pfx";
                    string certContent = "";

                    using (Stream stream = assembly.GetManifestResourceStream(certName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                         certContent = await reader.ReadToEndAsync();
                    }

                    X509Certificate2 cert = new X509Certificate2(
                        Encoding.Default.GetBytes(certContent),
                        WebAppConstants.CERT_PASSWORD,
                        X509KeyStorageFlags.MachineKeySet);

                    ClientAssertionCertificate cac = new ClientAssertionCertificate(
                        WebAppConstants.CLIENT_ID, cert);

                    var result = await authenticationContext.AcquireTokenAsync(
                        resource,
                        cac);
                    tokenForWebApplication = result.AccessToken;
                    break;

                default:
                    break;
            }

            return tokenForWebApplication;
        }

        /// <summary>
        /// Get Token for Native Application.
        /// </summary>
        /// <returns>Token for native application.</returns>
        public static async Task<string> GetTokenForNativeApplication(string resource)
        {
            string tokenForNativeApplication = string.Empty;

            AuthenticationContext authenticationContext = new AuthenticationContext(NativeAppConstants.AUTH_STRING, false);
            // Config for OAuth client credentials 
            ClientCredential clientCred = new ClientCredential(NativeAppConstants.CLIENT_ID,
                NativeAppConstants.CLIENT_SECRET);
            AuthenticationResult authenticationResult =
                await authenticationContext.AcquireTokenAsync(resource,
                    clientCred);
            tokenForNativeApplication = authenticationResult.AccessToken;
            
            return tokenForNativeApplication;
        }

        /// <summary>
        /// Get Token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        public static async Task<string> GetTokenForUser(string resource)
        {
            var redirectUri = new Uri("https://localhost");
            AuthenticationContext authenticationContext = new AuthenticationContext(UserModeConstants.AUTH_STRING, false);
            AuthenticationResult userAuthnResult = await authenticationContext.AcquireTokenAsync(resource,
                UserModeConstants.CLIENT_ID, redirectUri, new PlatformParameters(PromptBehavior.RefreshSession));
            string TokenForUser = userAuthnResult.AccessToken;

            WriteLine($"\n Welcome {userAuthnResult.UserInfo.GivenName} {userAuthnResult.UserInfo.FamilyName}");

            return TokenForUser;
        }
    }
}
