using MusicTransify.src.Services.Common;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Utilities.Security;
using System;
using System.Text.Json;

namespace MusicTransify.src.Services.YouTubeMusic
{
    public class YouTubeMusicAuthService : AuthService
    {
        private readonly HttpService _httpService;
        private readonly CookiesService _cookiesService;
        private readonly AuthHelper _authHelper;

        public YouTubeMusicAuthService(
            HttpService httpService,
            CookiesService cookiesService,
            AuthHelper authHelper
        ) : base(httpService, cookiesService, authHelper)
        {
            _httpService = httpService;
            _cookiesService = cookiesService;
            _authHelper = authHelper;
        }

        public override string GetLogInURI()
        {
            throw new NotImplementedException();
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