using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static System.Console;
using static System.IO.Path;

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
                    var certFile =  Combine(Environment.CurrentDirectory, "Resources", "EXCoreDemoWeb.pfx");

                    X509Certificate2 cert = new X509Certificate2(
                        certFile,
                        WebAppConstants.CERT_PASSWORD);

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
        public static async Task<string> GetTokenForNativeApplication(string resource, AuthenticationMethod auMethod)
        {
            string tokenForNativeApplication = string.Empty;

            AuthenticationContext authenticationContext = new AuthenticationContext(NativeAppConstants.AUTH_STRING, false);

            switch (auMethod)
            {
                case AuthenticationMethod.Password:
                    /******************************************************************************************
                     * AcquireToken(string resource, ClientCredential clientCredential) works with Web Apps. 
                     * It is easier to setup and is easy to use once setup. 
                     * Native Client Apps do not support this approach, because they lack a Client Secret. 
                     * [We can create one but it still have no permission.]
                     ******************************************************************************************/
                    // Config for OAuth client credentials 
                    ClientCredential clientCred = new ClientCredential(NativeAppConstants.CLIENT_ID,
                        NativeAppConstants.CLIENT_SECRET);
                    AuthenticationResult authenticationResult =
                        await authenticationContext.AcquireTokenAsync(resource,
                            clientCred);
                    tokenForNativeApplication = authenticationResult.AccessToken;
                    break;

                case AuthenticationMethod.Cert:
                    /******************************************************************************************
                     * AcquireToken(string resource, ClientAssertionCertificate clientCertificate) works with Web Apps. 
                     * The disadvantage of this approach is that it is harder to setup initially (PowerShell, certificate creation). 
                     * The advantage is that once setup it is easy to use. With Native Client Apps it does not work and throws this error: 
                     * AADSTS50012: Client is public so a 'client_assertion' should not be presented.
                     * 
                     * So here I will meet this issue
                     ******************************************************************************************/
                    var certFile = Combine(Environment.CurrentDirectory, "Resources", "EXCoreDemoNative.pfx");

                    X509Certificate2 cert = new X509Certificate2(
                        certFile,
                        NativeAppConstants.CERT_PASSWORD);

                    ClientAssertionCertificate cac = new ClientAssertionCertificate(
                        NativeAppConstants.CLIENT_ID, cert);

                    var result = await authenticationContext.AcquireTokenAsync(
                        resource,
                        cac);
                    tokenForNativeApplication = result.AccessToken;
                    break;

                default:
                    break;
            }
            
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

            WriteLine($"\nWelcome {userAuthnResult.UserInfo.GivenName} {userAuthnResult.UserInfo.FamilyName}");

            return TokenForUser;
        }
    }
}
