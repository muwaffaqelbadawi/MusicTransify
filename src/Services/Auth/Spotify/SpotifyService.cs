using System;
using System.Text.Json;
using MusicTransify.src.Contracts.HTTP;
using MusicTransify.src.Contracts.Spotify;
using MusicTransify.src.Utilities.Helper.Auth.Common;
using MusicTransify.src.Utilities.Helper.Auth.Spotify;

namespace MusicTransify.src.Services.Auth.Spotify
{
    public class SpotifyService : ISpotifyService
    {
        private readonly IHttpService _httpService;
        private readonly SpotifyAuthHelper _spotifyAuthHelper;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<SpotifyService> _logger;
        public SpotifyService(
            IHttpService httpService,
            SpotifyAuthHelper spotifyAuthHelper,
            AuthHelper authHelper,
            ILogger<SpotifyService> logger
        )
        {
            _httpService = httpService;
            _spotifyAuthHelper = spotifyAuthHelper;
            _authHelper = authHelper;
            _logger = logger;
        }

        public string GetLoginUri()
        {
            _logger.LogInformation("Accessing Spotify login Uri function");

            // Build login query
            var queryParameters = _spotifyAuthHelper.BuildLogin();

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
            var requestBody = _spotifyAuthHelper.BuildAuthExchange(code);

            // Set client name
            string clientName = _spotifyAuthHelper.ClientName;

            // Set tokenUri
            string tokenUri = _spotifyAuthHelper.TokenUri;

            try
            {
                // Get access token
                var accessToken = await _httpService.PostFormUrlEncodedContentAsync(
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
            var requestBody = _spotifyAuthHelper.BuildNewToken(refreshToken);
        
            // Set client name
            string clientName = _spotifyAuthHelper.ClientName;

            // Set tokenUri
            string tokenUri = _spotifyAuthHelper.TokenUri;

            try
            {
                // Get new access token
                var newAccessToken = await _httpService.PostFormUrlEncodedContentAsync(
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