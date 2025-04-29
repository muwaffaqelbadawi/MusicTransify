using System;
using System.Text.Json;
using SpotifyWebAPI_Intro.Utilities;

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
            string clientID = _optionsService.SpotifyClientId;

            // Set Response Type
            const string responseType = "code";

            // Set the scope value
            const string SCOPE = "user-read-private user-read-email";

            // Set Redirect URI
            string redirectURI = _optionsService.SpotifyRedirectUri;

            // Show dialog flag
            string showVal = "true";

            // Set OAuth state
            string state = _authHelper.GenerateSecureRandomString(32);

            // Store securely
            _httpService.AppendCookies(state);

            // Set Auth URL (base URL)
            string AuthURL = _optionsService.SpotifyAuthUrl;

            // Query Parameters
            var queryParameters = new Dictionary<string, string>
            {
                { "response_type", responseType },
                { "client_id", clientID },
                { "scope", SCOPE },
                { "redirect_uri", redirectURI },
                { "show_dialog", showVal },
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
            string grantType = "authorization_code";

            // Set Redirect URI
            string redirectURI = _optionsService.SpotifyRedirectUri;

            // Set Client ID
            string clientID = _optionsService.SpotifyClientId;

            // Set Client Secret
            string clientSecret = _optionsService.SpotifyClientSecret;

            // Set Token URL
            string tokenURL = _optionsService.SpotifyTokenUrl;

            // Build the request body
            var requestBody = new Dictionary<string, string>
            {
              { "code", Code },
              { "grant_type", grantType },
              { "redirect_uri", redirectURI },
              { "client_id", clientID },
              { "client_secret", clientSecret }
            };

            var TokenInfo = await _httpService.PostFormUrlEncodedContentAsync(tokenURL, requestBody);

            return TokenInfo;
        }

        public async Task<JsonElement> GetNewTokenAsync(string refreshToken)
        {
            // Set the grant_type
            string grantType = "refresh_token";

            // Set Client ID
            string clientID = _optionsService.SpotifyClientId;

            // Set Client Secret
            string clientSecret = _optionsService.SpotifyClientSecret;

            // Set Token URL
            string tokenURL = _optionsService.SpotifyTokenUrl;

            // Initialize request body
            var requestBody = new Dictionary<string, string>
            {
              { "grant_type", grantType },
              { "refresh_token", refreshToken },
              { "client_id", clientID },
              { "client_secret", clientSecret }
            };

            // Set Token Info
            var TokenInfo = await _httpService.PostFormUrlEncodedContentAsync(tokenURL, requestBody);

            return TokenInfo;
        }
    }
}