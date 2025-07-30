using System;
using System.Text.Json;
using MusicTransify.src.Utilities.Auth.Common;
using MusicTransify.src.Utilities.Auth.Spotify;
using MusicTransify.src.Contracts.Services.Auth.Spotify;
using MusicTransify.src.Contracts.Services.Http.Spotify;

namespace MusicTransify.src.Services.Auth.Spotify
{
    public class SpotifyService : ISpotifyService
    {
        // Remember to always inject the interface not the implementation
        // This allows for easier testing and mocking
        private readonly ISpotifyHttpService _spotifyHttpService;
        private readonly SpotifyHelper _spotifyAuthHelper;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<SpotifyService> _logger;
        public SpotifyService(
            ISpotifyHttpService spotifyHttpService,
            SpotifyHelper spotifyAuthHelper,
            AuthHelper authHelper,
            ILogger<SpotifyService> logger
        )
        {
            _spotifyHttpService = spotifyHttpService;
            _spotifyAuthHelper = spotifyAuthHelper;
            _authHelper = authHelper;
            _logger = logger;
        }

        public string GetLoginUri()
        {
            _logger.LogInformation("Accessing Spotify login Uri function");

            // Build login query
            Dictionary<string, string> queryParameters = _spotifyAuthHelper.BuildLoginRequest();

            // Transform login query to query string
            string queryString = _authHelper.ToQueryString(queryParameters);

            // Set authUri
            string authUri = _spotifyAuthHelper.AuthUri;

            // Build login URI
            return _authHelper.FormRedirectUrl(authUri, queryString);
        }

        public async Task<JsonElement> ExchangeAuthorizationCodeAsync(string code)
        {
            _logger.LogInformation("Accessing Exchanging authorization code function");

            // Build auth exchange query
            Dictionary<string, string> requestBody = _spotifyAuthHelper.BuildCodeExchangeRequest(code);

            // Set client name
            string clientName = _spotifyAuthHelper.ClientName;

            // Set tokenUri
            string tokenUri = _spotifyAuthHelper.TokenUri;

            try
            {
                // Get access token
                JsonElement accessToken = await _spotifyHttpService.PostFormUrlEncodedContentAsync(
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

        public async Task<JsonElement> GetNewTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Accessing New token generation request function");

            // Build new token query
            Dictionary<string, string> requestBody = _spotifyAuthHelper.BuildRefreshTokenRequest(refreshToken);
        
            // Set client name
            string clientName = _spotifyAuthHelper.ClientName;

            // Set tokenUri
            string tokenUri = _spotifyAuthHelper.TokenUri;

            try
            {
                // Get new access token
                JsonElement newAccessToken = await _spotifyHttpService.PostFormUrlEncodedContentAsync(
                    clientName: clientName,
                    tokenUri: tokenUri,
                    requestBody: requestBody
                );

                return newAccessToken;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get new access token.");
                throw new ApplicationException("Spotify token refresh failed.", ex);
            }
        }
    }
}