using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SpotifyWebAPI_Intro.utilities;

namespace SpotifyWebAPI_Intro.Services
{
    public class AuthService
    {
        private readonly OptionsService _optionsService;
        private readonly AuthHelper _authHelper;
        public AuthService(OptionsService optionsService, AuthHelper authHelper)
        {
            _optionsService = optionsService;
            _authHelper = authHelper;
        }

        public string GetLogInURL()
        {
            string ClientID = _optionsService.SpotifyClientId;

            // Set Response Type
            const string ResponseType = "code";

            // Set the scope value
            const string SCOPE = "user-read-private user-read-email";

            // Set Redirect URI
            string RedirectURI = _optionsService.SpotifyRedirectUri;

            // Set Auth URL
            string AuthURL = _optionsService.SpotifyAuthUrl;

            // Querry Parameters
            var queryParameters = new Dictionary<string, string>
            {
                { "client_id", ClientID },
                { "response_type", ResponseType },
                { "scope", SCOPE },
                { "redirect_uri", RedirectURI },
                { "show_dialog", "true" }
            };

            // Build the query string from the parameters
            var queryString = _authHelper.ToQueryString(queryParameters);

            // Returning the authorization URL
            return $"{AuthURL}?{queryString}";
        }
    }
}