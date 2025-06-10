using System;
using System.Text.Json;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Utilities.Security;

namespace MusicTransify.src.Services.Common
{
    public abstract class AuthService
    {
        private readonly HttpService _httpService;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<AuthService> _logger;
        public AuthService(HttpService httpService, CookiesService cookiesService, AuthHelper authHelper, ILogger<AuthService> logger)
        {
            _httpService = httpService;
            _cookiesService = cookiesService;
            _authHelper = authHelper;
            _logger = logger;
        }

        public abstract string GetLogInURI();
        public abstract Task<JsonElement> ExchangeAuthorizationCodeAsync(string authorizationCode);
        public abstract Task<JsonElement> GetNewTokenAsync(string refreshToken);
    }
}