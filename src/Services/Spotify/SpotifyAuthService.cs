using System;
using System.Text.Json;
using SpotifyWebAPI_Intro.src.Configurations.Spotify;
using SpotifyWebAPI_Intro.src.Services.Common;
using SpotifyWebAPI_Intro.src.Services.Cookies;
using SpotifyWebAPI_Intro.src.Utilities.Security;

namespace SpotifyWebAPI_Intro.Services.Spotify
{
    public class SpotifyAuthService
    {
        private readonly SpotifyOptionsProvider _spotifyOptionsProvider;
        private readonly HttpService _httpService;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;
        public SpotifyAuthService(SpotifyOptionsProvider spotifyOptionsProvider, HttpService httpService, CookiesService cookiesService, AuthHelper authHelper)
        {
            _spotifyOptionsProvider = spotifyOptionsProvider;
            _httpService = httpService;
            _cookiesService = cookiesService;
            _authHelper = authHelper;
        }

        public string GetLogInURI()
        {
            string clientID = _spotifyOptionsProvider.ClientId;

            // Set Response Type
            string responseType = _spotifyOptionsProvider.ResponseType;

            // Set the scope value
            string Scope = _spotifyOptionsProvider.Scope;

            // Set Redirect URI
            string redirectURI = _spotifyOptionsProvider.RedirectUri;

            // Show dialog flag
            string showDialog = _spotifyOptionsProvider.ShowDialog;

            // Set OAuth state
            string state = _authHelper.GenerateSecureRandomString(32);

            // Append cookies
            _cookiesService.AppendCookies(state);

            // Set Auth URL (base URL)
            string AuthURL = _spotifyOptionsProvider.AuthUri;

            // Query Parameters
            var queryParameters = new Dictionary<string, string>
            {
                { "response_type", responseType },
                { "client_id", clientID },
                { "scope", Scope },
                { "redirect_uri", redirectURI },
                { "show_dialog", showDialog },
                { "state", state }
            };

            // Build the query string from the parameters
            var queryString = _authHelper.ToQueryString(queryParameters);

            // Returning the authorization URL
            return $"{AuthURL}?{queryString}";
        }

        public async Task<JsonElement> ExchangeAuthorizationCodeAsync(string authorizationCode)
        {
            // Set the Grant Type
            string grantType = _spotifyOptionsProvider.GrantType;

            // Set Redirect URI
            string redirectURI = _spotifyOptionsProvider.RedirectUri;

            // Set Client ID
            string clientID = _spotifyOptionsProvider.ClientId;

            // Set Client Secret
            string clientSecret = _spotifyOptionsProvider.ClientSecret;

            // Set Token URL
            string tokenURL = _spotifyOptionsProvider.TokenUri;

            // Build the request body
            var requestBody = new Dictionary<string, string>
            {
              { "code", authorizationCode },
              { "grant_type", grantType },
              { "redirect_uri", redirectURI },
              { "client_id", clientID },
              { "client_secret", clientSecret }
            };

            var tokenInfo = await _httpService.PostFormUrlEncodedContentAsync(tokenURL, requestBody);

            return tokenInfo;
        }

        public async Task<JsonElement> GetNewTokenAsync(string refreshToken)
        {
            // Set the grant_type
            string grantType = _spotifyOptionsProvider.GrantType;

            // Set Client ID
            string clientID = _spotifyOptionsProvider.ClientId;

            // Set Client Secret
            string clientSecret = _spotifyOptionsProvider.ClientSecret;

            // Set Token URL
            string tokenURL = _spotifyOptionsProvider.TokenUri;

            // Initialize request body
            var requestBody = new Dictionary<string, string>
            {
              { "grant_type", grantType },
              { "refresh_token", refreshToken },
              { "client_id", clientID },
              
              // client_secret detected
              { "client_secret", clientSecret }
            };

            // Set Token Info
            var tokenInfo = await _httpService.PostFormUrlEncodedContentAsync(tokenURL, requestBody);

            return tokenInfo;
        }
    }
}