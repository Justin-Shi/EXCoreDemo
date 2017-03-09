using System;

namespace EXCoreDemo.AccessToken
{
    internal class WebAppConstants
    {
        public const string CLIENT_ID = "abc7c213-39b0-4d85-a522-e93002fec8ff";
        public const string CLIENT_SECRET = "kP8pRl/dpFJgiY7Yo/WuCww8rpdmEgfJpxaXD23ccD0=";
        public const string AUTH_STRING = ResourceConstants.AUTH_URI + TenantConstants.TENANT_NAME;
        public const string CERT_PASSWORD = "Pa$$word";
    }

    internal class NativeAppConstants
    {
        public const string CLIENT_ID = "b89a5665-af2b-4340-bb08-6ce791311319";
        public const string CLIENT_SECRET = "v7k4Yu8L1Vk9gDowYudb3hEzGlg3s30F68Bn/8BcxG0=";
        public const string AUTH_STRING = ResourceConstants.AUTH_URI + TenantConstants.TENANT_NAME;
        public const string CERT_PASSWORD = "Pa$$word";
    }

    internal class UserModeConstants
    {
        public const string CLIENT_ID = "b89a5665-af2b-4340-bb08-6ce791311319";
        public const string AUTH_STRING = ResourceConstants.AUTH_URI + "common/";
    }

    internal class ResourceConstants
    {
        public const string AUTH_URI = "https://login.microsoftonline.com/";
        public const string AZUREAD_GRAPH_RESOURCE_URI = "https://graph.windows.net/";
        public const string MICROSOFT_GRAPH_RESOURCE_URI = "https://graph.microsoft.com/";
        public const string EXCHANGE_RESOURCE_URI = "https://outlook.office365.com/";
        public const string GRAPH_SERVICE_OBJECTID = "00000002-0000-0000-c000-000000000000";
    }

    internal class TenantConstants
    {
        public const string TENANT_NAME = "sim-demo.sa4sp.com";
        public const string TENANT_ID = "da13a867-427b-4caf-b0c3-66cb86ef2bfb";
    }
}