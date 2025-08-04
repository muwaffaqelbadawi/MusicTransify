using System;
using System.Text.Json;
using MusicTransify.src.Utilities.Auth.Common;
using MusicTransify.src.Utilities.Auth.YouTubeMusic;
using MusicTransify.src.Contracts.Services.Auth.YouTubeMusic;
using MusicTransify.src.Contracts.Services.Http.YouTubeMusic;
using MusicTransify.src.Contracts.DTOs.Response.Token.YouTubeMusic;

namespace MusicTransify.src.Services.Auth.YouTubeMusic
{
    public class YouTubeMusicService : IYouTubeMusicService
    {
        // Remember to always inject the interface not the implementation
        // This allows for easier testing and mocking
        private readonly IYouTubeMusicHttpService _youTubeMusicHttpService;
        private readonly AuthHelper _authHelper;
        private readonly YouTubeMusicHelper _youTubeMusicAuthHelper;
        private readonly ILogger<YouTubeMusicService> _logger;

        public YouTubeMusicService(
            IYouTubeMusicHttpService youTubeMusicHttpService,
            AuthHelper authHelper,
            YouTubeMusicHelper youTubeMusicAuthHelper,
            ILogger<YouTubeMusicService> logger
        )
        {
            _youTubeMusicHttpService = youTubeMusicHttpService;
            _authHelper = authHelper;
            _youTubeMusicAuthHelper = youTubeMusicAuthHelper;
            _logger = logger;
        }

        public string GetLoginUri()
        {
            _logger.LogInformation("Accessing YouTube Music login Uri function");

            // Build login query
            Dictionary<string, string> queryParameters = _youTubeMusicAuthHelper.BuildLoginRequest();

            // Transform login query to query string
            string queryString = _authHelper.ToQueryString(queryParameters);

            // Set authUri
            string authUri = _youTubeMusicAuthHelper.AuthUri;

            // Build login URI
            return _authHelper.FormRedirectUrl(authUri, queryString);
        }

        public async Task<YouTubeMusicTokenResponse> ExchangeAuthorizationCodeAsync(string code)
        {
            _logger.LogInformation("Accessing Exchanging authorization code function");

            // Build auth exchange query
            Dictionary<string, string> requestBody = _youTubeMusicAuthHelper.BuildCodeExchangeRequest(code);

            // Set client name
            string clientName = _youTubeMusicAuthHelper.ClientName;

            // Set tokenUri
            string tokenUri = _youTubeMusicAuthHelper.TokenUri;

            try
            {
                // Get access token
                var accessToken = await _youTubeMusicHttpService.PostFormUrlEncodedContentAsync<YouTubeMusicTokenResponse>(
                clientName: clientName,
                tokenUri: tokenUri,
                requestBody: requestBody
                );

                return accessToken;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to exchange authorization code.");
                throw new ApplicationException("Spotify exchange failed.", ex);
            }
        }

        public async Task<YouTubeMusicTokenResponse> GetNewTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Accessing New token generation request function");

            // Build new token query
            Dictionary<string, string> requestBody = _youTubeMusicAuthHelper.BuildRefreshTokenRequest(refreshToken);

            // Set client name
            string clientName = _youTubeMusicAuthHelper.ClientName;

            // Set tokenUri
            string tokenUri = _youTubeMusicAuthHelper.TokenUri;

            try
            {
                // Get new access token
                var newAccessToken = await _youTubeMusicHttpService.PostFormUrlEncodedContentAsync<YouTubeMusicTokenResponse>(
                    clientName: clientName,
                    tokenUri: tokenUri,
                    requestBody: requestBody
                );

                return newAccessToken;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get new access token.");
                throw new ApplicationException("YouTube Music token refresh failed.", ex);
            }
        }
    }
}