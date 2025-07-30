using System;
using Microsoft.Extensions.Options;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Contracts.DTOs.YouTubeMusic;
using MusicTransify.src.Contracts.Helper.YouTubeMusic;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Utilities.Auth.Common;
using MusicTransify.src.Utilities.Security;

namespace MusicTransify.src.Utilities.Auth.YouTubeMusic
{
    public class YouTubeMusicHelper : IYouTubeMusicHelper
    {
        private readonly YouTubeMusicOptions _options;
        private readonly StateHelper _stateHelper;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<YouTubeMusicHelper> _logger;

        public YouTubeMusicHelper(
            IOptions<YouTubeMusicOptions> options,
            StateHelper stateHelper,
            CookiesService cookiesService,
            AuthHelper authHelper,
            ILogger<YouTubeMusicHelper> logger
        )
        {
            _options = options.Value;
            _stateHelper = stateHelper;
            _cookiesService = cookiesService;
            _authHelper = authHelper;
            _logger = logger;
        }

        public Dictionary<string, string> BuildLoginRequest()
        {
            _logger.LogInformation("Building YouTube Music login URL...");

            if (_options.Scope == null || _options.Scope.Length == 0)
            {
                throw new InvalidOperationException("YouTbe Music scopes are not configured");
            }

            // Set the client ID
            string clientID = _options.ClientId;

            // Set Redirect URI
            string redirectUri = _options.RedirectUri;

            // Set Redirect URI
            string responseType = _options.ResponseType;

            // Set access type
            string accessType = _options.AccessType;

            // Set access type
            string includeGrantedScopes = _options.IncludeGrantedScopes;

            // Set the list of scopes
            string[] scopeList = _options.Scope.Distinct().ToArray();
            _logger.LogInformation("Raw scopeList: {Scopes}", string.Join(", ", scopeList));

            // Set the scope list
            string scope = _authHelper.BuildScopeString(scopeList);
            _logger.LogInformation("YouTube Music scope: {Scope}", scope);

            // Set state
            string state = _stateHelper.GenerateSecureRandomString(32);

            // set cookies
            _cookiesService.AppendCookies(state);

            LoginRequestDto loginRequest = new()
            {
                ClientId = clientID,
                RedirectUri = redirectUri,
                ResponseType = responseType,
                Scope = scope,
                AccessType = accessType,
                IncludeGrantedScopes = includeGrantedScopes,
                State = state
            };

            return loginRequest.ToDictionary();
        }

        public Dictionary<string, string> BuildCodeExchangeRequest(string code)
        {
            _logger.LogInformation("Exchanging code for tokens...");

            // Set Client ID
            string clientId = _options.ClientId;

            // Set Client Secret
            string clientSecret = _options.ClientSecret;

            // Set Redirect URI
            string redirectUri = _options.RedirectUri;

            // Set the Grant Type
            string grantType = _options.GrantType;

            TokenExchangeRequestDto CodeExchangeRequest = new()
            {
                Code = code,
                ClientId = clientId,
                ClientSecret = clientSecret,
                RedirectUri = redirectUri,
                GrantType = grantType,
            };

            return CodeExchangeRequest.ToDictionary();
        }

        public Dictionary<string, string> BuildRefreshTokenRequest(string refreshToken)
        {
            _logger.LogInformation("Refreshing access token...");

            // Set Client ID
            string clientId = _options.ClientId;

            // Set Client Secret
            string clientSecret = _options.ClientSecret;

            // Set grant type for refresh token
            string refreshTokenGrantType = _options.RefreshTokenGrantType;

            RefreshTokenRequestDto refreshTokenRequest = new()
            {
                RefreshToken = refreshToken,
                ClientId = clientId,
                ClientSecret = clientSecret,
                RefreshTokenGrantType = refreshTokenGrantType
            };

            return refreshTokenRequest.ToDictionary();
        }

        public string ClientName => _options.ClientName;
        public string AuthUri => _options.AuthUri;
        public string TokenUri => _options.TokenUri;
    }
}