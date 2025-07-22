using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Contracts.YouTubeMusic;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Utilities.Security;

namespace MusicTransify.src.Services.Auth.YouTubeMusic
{
    public class YouTubeMusicService : IYouTubeMusicService
    {
        private readonly YouTubeMusicOptions _options;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;

        public YouTubeMusicService(
            IOptions<YouTubeMusicOptions> options,
            CookiesService cookiesService,
            AuthHelper authHelper)
        {
            _options = options.Value;
            _cookiesService = cookiesService;
            _authHelper = authHelper;
        }

        public string GetLoginUri()
        {
            // Set the client ID
            string clientID = _options.ClientId;

            // Set the client secret
            string clientSecret = _options.ClientSecret;

            // Set the response type
            string responseType = _options.ResponseType;

            // Set the scope list
            var scopeList = _options.Scope;

            // Join the scope list into a single string
            // This is necessary because the scope parameter in the OAuth URL expects a space-separated string
            var scope = string.Join(" ", scopeList);

            // Set the scope value
            string Scope = scope;

            // Set the scope value
            string accessType = _options.AccessType;

            // Set Redirect URI
            string redirectURI = _options.RedirectUri;

            // Set Auth URL (base URL)
            string AuthURL = _options.AuthUri;

            // Set OAuth state
            string state = _authHelper.GenerateSecureRandomString(32);

            // Set prompt
            string prompt = _options.Prompt;

            // Append cookies
            _cookiesService.AppendCookies(state);

            var queryParameters = new Dictionary<string, string>
            {
                { "client_id", clientID },
                { "client_secret", clientSecret },
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

        public Task<JsonElement> ExchangeAuthorizationCodeAsync(string authorizationCode)
        {
            throw new NotImplementedException();
        }

        public Task<JsonElement> GetNewTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}