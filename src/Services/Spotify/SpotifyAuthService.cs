using System;
using System.Text.Json;
using MusicTransify.src.Contracts;
using Microsoft.Extensions.Options;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Services.Common.Cookies;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Utilities.Security;

namespace MusicTransify.src.Services.Spotify
{
    public class SpotifyAuthService : IPlatformAuthService
    {
        private readonly SpotifyOptions _spotifyOptions;
        private readonly HttpService _httpService;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;

        public SpotifyAuthService(
            IOptions<SpotifyOptions> spotifyOptions,
            HttpService httpService,
            CookiesService cookiesService,
            AuthHelper authHelper)
        {
            _spotifyOptions = spotifyOptions.Value;
            _httpService = httpService ??
            throw new ArgumentNullException(nameof(httpService));
            _cookiesService = cookiesService ??
            throw new ArgumentNullException(nameof(cookiesService));
            _authHelper = authHelper ??
            throw new ArgumentNullException(nameof(authHelper));
        }

        public string GetLoginUri()
        {
            string clientID = _spotifyOptions.ClientId;

            // Set Response Type
            string responseType = _spotifyOptions.ResponseType;

            // Set the scope value
            string Scope = _spotifyOptions.Scope;

            // Set Redirect URI
            string redirectURI = _spotifyOptions.RedirectUri;

            // Show dialog flag
            string showDialog = _spotifyOptions.ShowDialog;

            // Set OAuth state
            string state = _authHelper.GenerateSecureRandomString(32);

            // Append cookies
            _cookiesService.AppendCookies(state);

            // Set Auth URL (base URL)
            string AuthURL = _spotifyOptions.AuthUri;

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
            string grantType = _spotifyOptions.GrantType;

            // Set Redirect URI
            string redirectURI = _spotifyOptions.RedirectUri;

            // Set Client ID
            string clientID = _spotifyOptions.ClientId;

            // Set Client Secret
            string clientSecret = _spotifyOptions.ClientSecret;

            // Set Token URL
            string tokenURL = _spotifyOptions.TokenUri;

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
            string grantType = _spotifyOptions.GrantType;

            // Set Client ID
            string clientID = _spotifyOptions.ClientId;

            // Set Client Secret
            string clientSecret = _spotifyOptions.ClientSecret;

            // Set Token URL
            string tokenURL = _spotifyOptions.TokenUri;

            // Initialize request body
            var requestBody = new Dictionary<string, string>
            {
              { "grant_type", grantType },
              { "refresh_token", refreshToken },
              { "client_id", clientID },
              { "client_secret", clientSecret }
            };

            // Set Token Info
            var tokenInfo = await _httpService.PostFormUrlEncodedContentAsync(tokenURL, requestBody);

            return tokenInfo;
        }
    }
}