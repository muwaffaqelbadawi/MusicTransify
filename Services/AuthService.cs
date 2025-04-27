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

            // Set Auth URL
            string AuthURL = _optionsService.SpotifyAuthUrl;

            // Query Parameters
            var QueryParameters = new Dictionary<string, string>
            {
                { "client_id", ClientID },
                { "response_type", ResponseType },
                { "scope", SCOPE },
                { "redirect_uri", RedirectURI },
                { "show_dialog", "true" }
            };

            // Build the query string from the parameters
            var QueryString = _authHelper.ToQueryString(QueryParameters);

            // Returning the authorization URL
            return $"{AuthURL}?{QueryString}";
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

            // Build the rquest body
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