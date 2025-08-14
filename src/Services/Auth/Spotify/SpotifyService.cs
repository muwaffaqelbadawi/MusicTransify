using System;
using MusicTransify.src.Api.Endpoints.DTOs.Responses.Token.Spotify;
using MusicTransify.src.Contracts.Services.Auth.Spotify;
using MusicTransify.src.Contracts.Services.Http.Spotify;
using MusicTransify.src.Contracts.Utilities.Spotify;
using MusicTransify.src.Utilities.Auth.Common;
using MusicTransify.src.Utilities.Options.Spotify;

namespace MusicTransify.src.Services.Auth.Spotify
{
    public class SpotifyService : ISpotifyService
    {
        private readonly ISpotifyHttpService _httpService;
        private readonly ISpotifyHelper _spotifyHelper;
        private readonly SpotifyOptionsHelper _options;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<SpotifyService> _logger;
        public SpotifyService(
            ISpotifyHttpService httpService,
            ISpotifyHelper spotifyHelper,
            SpotifyOptionsHelper options,
            AuthHelper authHelper,
            ILogger<SpotifyService> logger
        )
        {
            _httpService = httpService;
            _spotifyHelper = spotifyHelper;
            _options = options;
            _authHelper = authHelper;
            _logger = logger;
        }

        public string GetLoginUri()
        {
            _logger.LogInformation("Accessing Spotify login Uri function");

            // Build login query
            Dictionary<string, string> queryParameters = _spotifyHelper.BuildLoginRequest();

            // Transform login query to query string
            string queryString = _authHelper.ToQueryString(queryParameters);

            // Set authUri
            string authUri = _options.AuthUri;

            // Build login URI
            return _authHelper.BuildRedirectUri(authUri, queryString);
        }

        public async Task<SpotifyTokenResponseDto> ExchangeAuthorizationCodeAsync(string code)
        {
            _logger.LogInformation("Accessing Exchanging authorization code function");

            // Build auth exchange query
            Dictionary<string, string> requestBody = _spotifyHelper.BuildCodeExchangeRequest(code);

            // Set tokenUri
            string tokenUri = _options.TokenUri;

            try
            {
                // Get access token
                var accessToken = await _httpService.PostFormUrlEncodedContentAsync<SpotifyTokenResponseDto>(
                    tokenUri: tokenUri,
                    requestBody: requestBody
                );

                if (string.IsNullOrEmpty(tokenUri))
                    throw new InvalidOperationException("Token URI is null or empty");

                if (requestBody is null)
                    throw new InvalidOperationException("Request body is null");

                if (accessToken is null)
                    throw new InvalidOperationException("Access token is null");

                return accessToken;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to exchange authorization code.");
                throw new HttpRequestException("Spotify exchange failed.", ex);
            }
        }

        public async Task<SpotifyTokenResponseDto> GetNewTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Accessing New token generation request function");

            // Build new token query
            Dictionary<string, string> requestBody = _spotifyHelper.BuildRefreshTokenRequest(refreshToken);

            // Set tokenUri
            string tokenUri = _options.TokenUri;

            try
            {
                // Get new access token
                var newAccessToken = await _httpService.PostFormUrlEncodedContentAsync<SpotifyTokenResponseDto>(
                    tokenUri: tokenUri,
                    requestBody: requestBody
                );

                return newAccessToken;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get new access token.");
                throw new HttpRequestException("Spotify token refresh failed.", ex);
            }
        }
    }
}