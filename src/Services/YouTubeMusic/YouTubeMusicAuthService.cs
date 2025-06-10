using System;
using System.Text.Json;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Utilities.Security;

namespace MusicTransify.src.Services.YouTubeMusic
{
    public class YouTubeMusicAuthService : AuthService
    {
        private readonly YouTubeMusicOptions _youTubeMusicOptions;
        private readonly HttpService _httpService;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<YouTubeMusicAuthService> _logger;

        public YouTubeMusicAuthService(
            YouTubeMusicOptions youTubeMusicOptions,
            HttpService httpService,
            CookiesService cookiesService,
            AuthHelper authHelper,
            ILogger<YouTubeMusicAuthService> logger
        ) : base(httpService, cookiesService, authHelper, logger)
        {
            _youTubeMusicOptions = youTubeMusicOptions;
            _httpService = httpService;
            _cookiesService = cookiesService;
            _authHelper = authHelper;
            _logger = logger;
        }

        public override string GetLogInURI()
        {
            // Set the client ID
            string clientID = _youTubeMusicOptions.ClientId;

            // Set Response Type
            string responseType = _youTubeMusicOptions.ResponseType;

            // Set the scope value
            string Scope = _youTubeMusicOptions.Scope;

            // Set the scope value
            string accessType = _youTubeMusicOptions.AccessType;

            // Set Redirect URI
            string redirectURI = _youTubeMusicOptions.RedirectUri;

            // Set Auth URL (base URL)
            string AuthURL = _youTubeMusicOptions.AuthUri;

            // Set OAuth state
            string state = _authHelper.GenerateSecureRandomString(32);

            // Set prompt
            string prompt = _youTubeMusicOptions.Prompt;

            // Append cookies
            _cookiesService.AppendCookies(state);

            var queryParameters = new Dictionary<string, string>
            {
                { "client_id", clientID },
                { "redirect_uri", redirectURI },
                { "response_type", responseType },
                { "scope", Scope },
                { "access_type", accessType },
                { "state", state },
                { "prompt", prompt },
            };

            // Build the query string from the parameters
            var queryString = _authHelper.ToQueryString(queryParameters);

            // Returning the authorization URL
            return $"{AuthURL}?{queryString}";
        }

        public override Task<JsonElement> ExchangeAuthorizationCodeAsync(string authorizationCode)
        {
            throw new NotImplementedException();
        }

        public override Task<JsonElement> GetNewTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}