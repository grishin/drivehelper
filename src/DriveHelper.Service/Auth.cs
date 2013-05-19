using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Drive.v2;
using Google.Apis.Util;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveHelper.Service
{
    internal class Auth
    {
        public static string ClientId;
        public static string ClientSecret;
        public static string RefreshToken;

        private static IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        {
            // Get the auth URL:
            IAuthorizationState state = new AuthorizationState(new[] { DriveService.Scopes.Drive.GetStringValue() });
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            if (!String.IsNullOrWhiteSpace(RefreshToken)) {
                state.RefreshToken = RefreshToken;

                if (arg.RefreshToken(state))
                    return state;
            }
            return state;
        }

        public static DriveService CreateDriveService()
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description, Auth.ClientId, Auth.ClientSecret);
            var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthorization);
            var service = new DriveService(new BaseClientService.Initializer() {
                Authenticator = auth
            });
            return service;
        }
    }
}
