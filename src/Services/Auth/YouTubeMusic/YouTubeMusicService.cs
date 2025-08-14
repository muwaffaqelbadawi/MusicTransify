using System;
using MusicTransify.src.Contracts.Services.Auth.YouTubeMusic;
using MusicTransify.src.Contracts.Services.Http.YouTubeMusic;
using MusicTransify.src.Contracts.Utilities.YouTubeMusic;
using MusicTransify.src.Utilities.Auth.Common;
using MusicTransify.src.Utilities.Options.YouTubeMusic;
using MusicTransify.src.Api.YouTubeMusic.Token.Responses;

namespace MusicTransify.src.Services.Auth.YouTubeMusic
{
    public class YouTubeMusicService : IYouTubeMusicService
    {
        private readonly IYouTubeMusicHttpService _httpService;
        private readonly IYouTubeMusicHelper _youtubeMusicHelper;
        private readonly YouTubeMusicOptionsHelper _options;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<YouTubeMusicService> _logger;

        public YouTubeMusicService(
            IYouTubeMusicHttpService httpService,
            IYouTubeMusicHelper youtubeMusicHelper,
            YouTubeMusicOptionsHelper options,
            AuthHelper authHelper,
            ILogger<YouTubeMusicService> logger
        )
        {
            _httpService = httpService;
            _youtubeMusicHelper = youtubeMusicHelper;
            _options = options;
            _authHelper = authHelper;
            _logger = logger;
        }

        public string GetLoginUri()
        {
            _logger.LogInformation("GET login Uri for YouTube API");

            // Build login query
            Dictionary<string, string> queryParameters = _youtubeMusicHelper.BuildLoginRequest();

            // Transform login query to query string
            string queryString = _authHelper.ToQueryString(queryParameters);

            // Set authUri
            string authUri = _options.AuthUri;

            // Build login URI
            return _authHelper.BuildRedirectUri(authUri, queryString);
        }

        public async Task<YouTubeMusicTokenResponseDto> ExchangeAuthorizationCodeAsync(string code)
        {
            _logger.LogInformation("Accessing Exchanging authorization code function");

            // Build auth exchange query
            Dictionary<string, string> requestBody = _youtubeMusicHelper.BuildCodeExchangeRequest(code);

            // Set tokenUri
            string tokenUri = _options.TokenUri;

            try
            {
                // Get access token
                var accessToken = await _httpService.PostFormUrlEncodedContentAsync<YouTubeMusicTokenResponseDto>(
                tokenUri: tokenUri,
                requestBody: requestBody
                );

                return accessToken;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to exchange authorization code.");
                throw new HttpRequestException("YouTube Music exchange failed.", ex);
            }
        }

        public async Task<YouTubeMusicTokenResponseDto> GetNewTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Accessing New token generation request function");

            // Build new token query
            Dictionary<string, string> requestBody = _youtubeMusicHelper.BuildRefreshTokenRequest(refreshToken);

            // Set tokenUri
            string tokenUri = _options.TokenUri;

            try
            {
                // Get new access token
                var newAccessToken = await _httpService.PostFormUrlEncodedContentAsync<YouTubeMusicTokenResponseDto>(
                    tokenUri: tokenUri,
                    requestBody: requestBody
                );

                return newAccessToken;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get new access token.");
                throw new HttpRequestException("YouTube Music token refresh failed.", ex);
            }
        }
    }
}