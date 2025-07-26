using System;
using Microsoft.Extensions.Options;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Utilities.Helper.Auth.Common;
using MusicTransify.src.Utilities.Security;

namespace MusicTransify.src.Utilities.Helper.Auth.Spotify
{
    public class SpotifyAuthHelper
    {
        private readonly SpotifyOptions _options;
        private readonly StateHelper _stateHelper;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<SpotifyAuthHelper> _logger;
        public SpotifyAuthHelper(
            IOptions<SpotifyOptions> options,
            StateHelper stateHelper,
            CookiesService cookiesService,
            AuthHelper authHelper,
            ILogger<SpotifyAuthHelper> logger
        )
        {
            _options = options.Value;
            _stateHelper = stateHelper;
            _cookiesService = cookiesService;
            _authHelper = authHelper;
            _logger = logger;
        }
        public Dictionary<string, string> BuildLogin()
        {
            _logger.LogInformation("Accessing Sporify login query builder function");

            if (_options.Scope == null || _options.Scope.Length == 0)
                throw new InvalidOperationException("Spotify scopes are not configured.");

            // Set Response Type
            string responseType = _options.ResponseType;

            // Set the client ID
            string clientID = _options.ClientId;

            // Set the scope list
            var scope = _authHelper.BuildScopeString(_options.Scope);

            _logger.LogInformation("Spotify scope: {}", scope);

            // Set Redirect URI
            string redirectURI = _options.RedirectUri;

            // Show dialog flag
            string showDialog = _options.ShowDialog;

            // Set state
            string state = _stateHelper.GenerateSecureRandomString(32);

            _cookiesService.AppendCookies(state);

            // Query Parameters
            return new Dictionary<string, string>
            {
                { "response_type", responseType },
                { "client_id", clientID },
                { "scope", scope },
                { "redirect_uri", redirectURI },
                { "show_dialog", showDialog },
                { "state", state }
            };
        }

        public Dictionary<string, string> BuildAuthExchange(string code)
        {
            _logger.LogInformation("Accessing Sporify auth exchange query builder function");

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

            // Build exchange
            return new Dictionary<string, string>
            {
              { "code", code },
              { "grant_type", grantType },
              { "redirect_uri", redirectURI },
              { "client_id", clientID },
              { "client_secret", clientSecret }
            };
        }
        public Dictionary<string, string> BuildNewToken(string refreshToken)
        {
            _logger.LogInformation("Accessing Sporifynew token query builder function");

            // Set the grant_type
            string grantType = _options.GrantType;

            // Set Client ID
            string clientID = _options.ClientId;

            // Set Client Secret
            string clientSecret = _options.ClientSecret;

            // Set Token URL
            string tokenURL = _options.TokenUri;

            return new Dictionary<string, string>
            {
              { "grant_type", grantType },
              { "refresh_token", refreshToken },
              { "client_id", clientID },
              { "client_secret", clientSecret }
            };
        }
        public string ClientName => _options.ClientName ?? throw new InvalidOperationException("Client Name is not configured");
        public string AuthUri => _options.AuthUri ?? throw new InvalidOperationException("Auth URI is not configured.");
        public string TokenUri => _options.TokenUri ?? throw new InvalidOperationException("Token URI is not configured.");
    }
}
