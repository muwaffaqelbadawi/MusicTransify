using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MusicTransify.src.Contracts;
using MusicTransify.src.Utilities.Security;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Services.Cookies;

namespace MusicTransify.src.Services.Auth.Spotify
{
    public class SpotifyAuthService : IAuthService
    {
        private readonly SpotifyOptions _options;
        private readonly IHttpService _httpService;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<SpotifyAuthService> _logger;
        public SpotifyAuthService(
            IOptions<SpotifyOptions> options,
            IHttpService httpService,
            CookiesService cookiesService,
            AuthHelper authHelper,
            ILogger<SpotifyAuthService> logger
        )
        {
            _options = options.Value;
            _httpService = httpService;
            _cookiesService = cookiesService;
            _authHelper = authHelper;
            _cookiesService = cookiesService;
            _logger = logger;
        }

        public string GetLoginUri()
        {
            _logger.LogInformation("");

            string clientID = _options.ClientId;

            // Set Response Type
            string responseType = _options.ResponseType;

            // Set the scope value
            string Scope = _options.Scope;

            // Set Redirect URI
            string redirectURI = _options.RedirectUri;

            // Show dialog flag
            string showDialog = _options.ShowDialog;

            // Set OAuth state
            string state = _authHelper.GenerateSecureRandomString(32);

            // Append cookies
            // Store state in cookies instead of appending
            // _cookiesService.StoreState(state);
            _cookiesService.AppendCookies(state);

            // Set Auth URL (base URL)
            string AuthURL = _options.AuthUri;

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

        public async Task<JsonElement> ExchangeAuthorizationCodeAsync(string code)
        {
            _logger.LogInformation("");

            // Set the Grant Type
            string grantType = _options.GrantType;

            // Set Redirect URI
            string redirectURI = _options.RedirectUri;

            // Set Client ID
            string clientID = _options.ClientId;

            // Set Client Secret
            string clientSecret = _options.ClientSecret;

            // Set Token URL
            string tokenURL = _options.TokenUri;

            // Build the request body
            var requestBody = new Dictionary<string, string>
            {
              { "code", code },
              { "grant_type", grantType },
              { "redirect_uri", redirectURI },
              { "client_id", clientID },
              { "client_secret", clientSecret }
            };

            var tokenInfo = await _httpService.PostFormUrlEncodedContentAsync(
            "Spotify",
            tokenURL,
            requestBody
            );
            return tokenInfo;
        }

        public async Task<JsonElement> GetNewTokenAsync(string refreshToken)
        {
            _logger.LogInformation("");

            // Set the grant_type
            string grantType = _options.GrantType;

            // Set Client ID
            string clientID = _options.ClientId;

            // Set Client Secret
            string clientSecret = _options.ClientSecret;

            // Set Token URL
            string tokenURL = _options.TokenUri;

            // Initialize request body
            var requestBody = new Dictionary<string, string>
            {
              { "grant_type", grantType },
              { "refresh_token", refreshToken },
              { "client_id", clientID },
              { "client_secret", clientSecret }
            };

            var tokenInfo = await _httpService.PostFormUrlEncodedContentAsync(
            "Spotify",
            tokenURL,
            requestBody
            );
            return tokenInfo;
        }
    }
}