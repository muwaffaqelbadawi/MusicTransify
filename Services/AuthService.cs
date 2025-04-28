using System;
using System.Text.Json;
using SpotifyWebAPI_Intro.utilities;

namespace SpotifyWebAPI_Intro.Services
{
    public class AuthService
    {
        private readonly OptionsService _optionsService;
        private readonly HttpService _httpService;
        private readonly AuthHelper _authHelper;
        public AuthService(OptionsService optionsService, HttpService httpService, AuthHelper authHelper)
        {
            _optionsService = optionsService;
            _httpService = httpService;
            _authHelper = authHelper;
        }

        public string GetLogInURI()
        {
            string ClientID = _optionsService.SpotifyClientId;

            // Set Response Type
            const string ResponseType = "code";

            // Set the scope value
            const string SCOPE = "user-read-private user-read-email";

            // Set Redirect URI
            string RedirectURI = _optionsService.SpotifyRedirectUri;

            // Show dialog flag
            string ShowDialog = "true";

            // Set OAuth state
            string state = _authHelper.GenerateRandomString(16);

            // Append cookies to the response
            _httpService.AppendCookies(state);

            // Set Auth URL (base URL)
            string AuthURL = _optionsService.SpotifyAuthUrl;

            // Query Parameters
            var queryParameters = new Dictionary<string, string>
            {
                { "response_type", ResponseType },
                { "client_id", ClientID },
                { "scope", SCOPE },
                { "redirect_uri", RedirectURI },
                { "show_dialog", ShowDialog },
                { "state", state }
            };

            // Build the query string from the parameters
            var queryString = _authHelper.ToQueryString(queryParameters);

            // Returning the authorization URL
            return $"{AuthURL}?{queryString}";
        }

        public async Task<JsonElement> ExchangeAuthorizationCodeAsync(string Code)
        {
            // Set the Grant Type
            string GrantType = "authorization_code";

            // Set Redirect URI
            string RedirectURI = _optionsService.SpotifyRedirectUri;

            // Set Client ID
            string ClientID = _optionsService.SpotifyClientId;

            // Set Client Secret
            string ClientSecret = _optionsService.SpotifyClientSecret;

            // Set Token URL
            string TokenURL = _optionsService.SpotifyTokenUrl;

            // Build the request body
            var RequestBody = new Dictionary<string, string>
            {
              { "code", Code },
              { "grant_type", GrantType },
              { "redirect_uri", RedirectURI },
              { "client_id", ClientID },
              { "client_secret", ClientSecret }
            };

            var TokenInfo = await _httpService.PostFormUrlEncodedContentAsync(TokenURL, RequestBody);

            return TokenInfo;
        }

        public async Task<JsonElement> GetNewTokenAsync(string RefreshToken)
        {
            // Set the grant_type
            string GrantType = "refresh_token";

            // Set Client ID
            string ClientID = _optionsService.SpotifyClientId;

            // Set Client Secret
            string ClientSecret = _optionsService.SpotifyClientSecret;

            // Set Token URL
            string TokenURL = _optionsService.SpotifyTokenUrl;

            // Initialize request body
            var RequestBody = new Dictionary<string, string>
            {
              { "grant_type", GrantType },
              { "refresh_token", RefreshToken },
              { "client_id", ClientID },
              { "client_secret", ClientSecret }
            };

            // Set Token Info
            var TokenInfo = await _httpService.PostFormUrlEncodedContentAsync(TokenURL, RequestBody);

            return TokenInfo;
        }
    }
}