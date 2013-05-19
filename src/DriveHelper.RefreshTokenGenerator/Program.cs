using System;
using System.Diagnostics;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Drive.v2;
using Google.Apis.Util;
using Google.Apis.Services;
using DriveHelper.RefreshTokenGenerator.Properties;
using Google.Apis.Drive.v2.Data;

namespace DriveHelper.RefreshTokenGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description, Settings.Default.ClientId, Settings.Default.ClientSecret);
            var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthorization);
            var driveService = new DriveService(new BaseClientService.Initializer() {
                Authenticator = auth
            });
            driveService.Files.List().Fetch();
            Console.ReadLine();
        }

        private static IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        {
            // Get the auth URL:
            IAuthorizationState state = new AuthorizationState(new[] { DriveService.Scopes.Drive.GetStringValue() });
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);

            Uri authUri = arg.RequestUserAuthorization(state);

            // Request authorization from the user (by opening a browser window):
            Process.Start(authUri.ToString());
            Console.Write("  Authorization Code: ");
            string authCode = Console.ReadLine();
            Console.WriteLine();

            // Retrieve the access token by using the authorization code:
            var result = arg.ProcessUserAuthorization(authCode, state);
            
            Console.WriteLine("RefreshToken: {0}", state.RefreshToken);
            System.IO.File.WriteAllText("refreshtoken.txt", state.RefreshToken);

            return result;
        }        
    }
}