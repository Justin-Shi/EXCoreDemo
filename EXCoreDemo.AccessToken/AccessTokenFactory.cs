using System.Threading.Tasks;

namespace EXCoreDemo.AccessToken
{
    public class AccessTokenFactory
    {
        public static async Task<string> GetAccessTokenAsync(AuthenticationType auType, AuthenticationMethod auMethod, ResourceType resType)
        {
            string accessToken = string.Empty;

            switch(auType)
            {
                case AuthenticationType.User:
                    if(resType == ResourceType.AzureGraphApi)
                    {
                        accessToken = await AccessTokenHelper.GetTokenForUser(ResourceConstants.AZUREAD_GRAPH_RESOURCE_URI);
                    }
                    else if(resType == ResourceType.MicrosoftGraphApi)
                    {
                        accessToken = await AccessTokenHelper.GetTokenForUser(ResourceConstants.MICROSOFT_GRAPH_RESOURCE_URI);
                    }
                    else
                    {
                        accessToken = await AccessTokenHelper.GetTokenForUser(ResourceConstants.EXCHANGE_RESOURCE_URI);
                    }
                    break;

                case AuthenticationType.WebApp:
                    if (resType == ResourceType.AzureGraphApi)
                    {
                        accessToken = await AccessTokenHelper.GetTokenForWebApplication(ResourceConstants.AZUREAD_GRAPH_RESOURCE_URI, auMethod);
                    }
                    else if (resType == ResourceType.MicrosoftGraphApi)
                    {
                        accessToken = await AccessTokenHelper.GetTokenForWebApplication(ResourceConstants.MICROSOFT_GRAPH_RESOURCE_URI, auMethod);
                    }
                    else
                    {
                        accessToken = await AccessTokenHelper.GetTokenForWebApplication(ResourceConstants.EXCHANGE_RESOURCE_URI, auMethod);
                    }
                    break;

                case AuthenticationType.NativeApp:
                    if (resType == ResourceType.AzureGraphApi)
                    {
                        accessToken = await AccessTokenHelper.GetTokenForNativeApplication(ResourceConstants.AZUREAD_GRAPH_RESOURCE_URI);
                    }
                    else if (resType == ResourceType.MicrosoftGraphApi)
                    {
                        accessToken = await AccessTokenHelper.GetTokenForNativeApplication(ResourceConstants.MICROSOFT_GRAPH_RESOURCE_URI);
                    }
                    else
                    {
                        accessToken = await AccessTokenHelper.GetTokenForNativeApplication(ResourceConstants.EXCHANGE_RESOURCE_URI);
                    }
                    break;

                default:
                    break;
            }

            return accessToken;
        }
    }
}
