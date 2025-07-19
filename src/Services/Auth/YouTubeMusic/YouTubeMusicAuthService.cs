using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Contracts;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Utilities.Security;

namespace MusicTransify.src.Services.Auth.YouTubeMusic
{
    public class YouTubeMusicAuthService : IAuthService
    {
        private readonly YouTubeMusicOptions _options;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;

        public YouTubeMusicAuthService(
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

            // Set Response Type
            string responseType = _options.ResponseType;

            // Set the scope value
            string Scope = _options.Scope;

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