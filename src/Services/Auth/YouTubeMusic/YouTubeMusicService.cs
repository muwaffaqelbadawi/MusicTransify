using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Contracts.YouTubeMusic;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Utilities.Helper.Auth.Common;
using MusicTransify.src.Utilities.Helper.Auth.YouTubeMusic;
using MusicTransify.src.Utilities.Security;

namespace MusicTransify.src.Services.Auth.YouTubeMusic
{
    public class YouTubeMusicService : IYouTubeMusicService
    {
        private readonly YouTubeMusicOptions _options;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;
        private readonly YouTubeMusicAuthHelper _youTubeMusicAuthHelper;
        private readonly StateHelper _stateHelper;
        private readonly ILogger<YouTubeMusicService> _logger;

        public YouTubeMusicService(
            IOptions<YouTubeMusicOptions> options,
            CookiesService cookiesService,
            AuthHelper authHelper,
            YouTubeMusicAuthHelper youTubeMusicAuthHelper,
            StateHelper stateHelper,
            ILogger<YouTubeMusicService> logger
        )
        {
            _options = options.Value;
            _cookiesService = cookiesService;
            _authHelper = authHelper;
            _youTubeMusicAuthHelper = youTubeMusicAuthHelper;
            _stateHelper = stateHelper;
            _logger = logger;
        }

        public string GetLoginUri()
        {
            _logger.LogInformation("Accessing YouTube service");
            _logger.LogInformation("Accessing YouTube login URI");

            // Set the client ID
            string clientID = _options.ClientId;

            // Set the client secret
            string clientSecret = _options.ClientSecret;

            // Set the response type
            string responseType = _options.ResponseType;

            // Set the scope list
            var scope = _authHelper.BuildScopeString(_options.Scope);

            _logger.LogInformation("Spotify scope: {scope}", scope);

            // Set the scope value
            string accessType = _options.AccessType;

            // Set Redirect URI
            string redirectURI = _options.RedirectUri;

            // Set Auth URL (base URL)
            string AuthURL = _options.AuthUri;

            // Set state
            string state = _stateHelper.GenerateSecureRandomString(32);

            // Set prompt
            string prompt = _options.Prompt;

            // set cookies
            _cookiesService.AppendCookies(state);

            var queryParameters = new Dictionary<string, string>
            {
                { "client_id", clientID },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectURI },
                { "response_type", responseType },
                { "scope", scope },
                { "access_type", accessType },
                { "state", state },
                { "prompt", prompt },
            };

            // Build the query string from the parameters
            var queryString = _authHelper.ToQueryString(queryParameters);

            // Returning the authorization URL
            return $"{AuthURL}?{queryString}";
        }

        public Task<JsonElement> ExchangeAuthorizationCodeAsync(string code)
        {
            _logger.LogInformation("Exchanging authorization code for access token");

            
            throw new NotImplementedException();
        }

        public Task<JsonElement> GetNewTokenAsync(string refreshToken)
        {
            _logger.LogInformation("New token generation requested");
            throw new NotImplementedException();
        }

        public Task<JsonElement> ExchangeAuthorizationCodeAsync(
            string clientName,
            string code,
            string tokenUri
        )
        {
            throw new NotImplementedException();
        }

        public Task<JsonElement> GetNewTokenAsync(
            string clientName,
            string tokenUrl,
            string refreshToken
        )
        {
            throw new NotImplementedException();
        }
    }
}