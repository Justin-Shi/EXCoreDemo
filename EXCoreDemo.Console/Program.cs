using EXCoreDemo.AccessToken;
using EXCoreDemo.Core.Helper;
using EXCoreDemo.Core.MicrosoftGraph;
using System;
using System.Collections.Generic;
using System.Net;
using static System.Console;

namespace EXCoreDemo.Console
{
    class Program
    {
        // Single-Threaded Apartment required for OAuth2 Authz Code flow (User Authn) to execute for this demo app
        [STAThread]
        private static void Main()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            WriteLine("Run operations for signed-in user, or in app-only mode.\n");
            WriteLine("[w] - web app-only\n[n] - native app-only\n[u] - as user\n\nPlease enter your choice:\n");
            string accessToken = string.Empty;

            ConsoleKeyInfo key = ReadKey();
            switch (key.KeyChar)
            {
                case 'w':
                    WriteLine("\nRunning web app-only mode\n\n");
                    accessToken = AccessTokenFactory.GetAccessTokenAsync(AuthenticationType.WebApp, AuthenticationMethod.Password, ResourceType.MicrosoftGraphApi).Result;

                    Demo(accessToken);

                    break;
                case 'n':
                    WriteLine("\nRunning native app-only mode\n\n");
                    accessToken = AccessTokenFactory.GetAccessTokenAsync(AuthenticationType.NativeApp, AuthenticationMethod.Password, ResourceType.MicrosoftGraphApi).Result;

                    Demo(accessToken);

                    break;
                case 'u':
                    WriteLine("\nRunning in user mode\n\n");
                    accessToken = AccessTokenFactory.GetAccessTokenAsync(AuthenticationType.User, AuthenticationMethod.Password, ResourceType.MicrosoftGraphApi).Result;

                    Demo(accessToken);

                    break;
                default:
                    WriteError("\nSelection not recognized. Running in web app-only mode\n\n");
                    accessToken = AccessTokenFactory.GetAccessTokenAsync(AuthenticationType.WebApp, AuthenticationMethod.Password, ResourceType.MicrosoftGraphApi).Result;

                    Demo(accessToken);

                    break;
            }

            //*********************************************************************************************
            // End of Demo Console App
            //*********************************************************************************************

            WriteLine("\nCompleted at {0} \n Press Any Key to Exit.", DateTime.Now.ToUniversalTime());
            ReadKey();
        }

        public static string ExtractErrorMessage(Exception exception)
        {
            List<string> errorMessages = new List<string>();
            string tabs = "\n";
            while (exception != null)
            {
                tabs += "    ";
                errorMessages.Add(tabs + exception.Message);
                exception = exception.InnerException;
            }
            return string.Join("-\n", errorMessages);
        }

        public static void WriteError(string output, params object[] args)
        {
            ForegroundColor = ConsoleColor.Red;
            Error.WriteLine(output, args);
            ResetColor();
        }

        private static void Demo(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                WriteLine("Failed to get the access token!");
                return;
            }

            Users usersHandler = new Users(accessToken);
            WriteLine("Getting users ...");
            var users = usersHandler.GetAllUsersAsync().Result;
            foreach (var user in users)
            {
                WriteLine(Users.FormatListUser(user));
            }

            Groups groupsHandler = new Groups(accessToken);
            WriteLine($"{Environment.NewLine}Getting groups ...");
            var groups = groupsHandler.GetAllGroupsAsync().Result;
            foreach (var group in groups)
            {
                WriteLine(Groups.FormatListGroup(group));
            }

            var validUsers = users.FindAll(x => x.Mail.IsValidEmailAddress());
            int index = (new Random().Next(validUsers.Count) - 1);
            Messages messagesHanlder = new Messages(accessToken);
            var randomUser = validUsers[index];
            WriteLine($"{Environment.NewLine}Getting {randomUser.DisplayName}'s messages ...");
            var messages = messagesHanlder.GetMessagesByUserAsync(randomUser.Id).Result;
            foreach (var message in messages)
            {
                WriteLine(Messages.FormatListMessage(message));
            }
        }
    }
}
