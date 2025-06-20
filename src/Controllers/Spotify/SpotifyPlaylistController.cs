using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Controllers.Common;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Utilities.Common;

namespace MusicTransify.src.Controllers.Spotify
{
    [ApiController]
    [Route("playlists")] // Route: "/playlists"
    public class SpotifyPlaylistsController : AuthController
    {
        private readonly SpotifyOptions _spotifyOptions;
        private readonly new SessionService _sessionService;
        private readonly HttpService _httpService;
        private readonly TokenHelper _tokenHelper;
        private readonly new ILogger<SpotifyPlaylistsController> _logger;

        public SpotifyPlaylistsController(
            SpotifyOptions spotifyOptions,
            AuthService authService,
            SessionService sessionService,
            HttpService httpService,
            TokenHelper token,
            ILogger<SpotifyPlaylistsController> logger,
            ILogger<AuthController> baseLogger
        ) : base(authService, sessionService, baseLogger)
        {
            _spotifyOptions = spotifyOptions;
            _sessionService = sessionService;
            _httpService = httpService;
            _tokenHelper = token;
            _logger = logger;
        }


        [HttpGet] // Route: "/playlists"
        public async Task<IActionResult> GetPlaylistsAsync()
        {
            // Use the log information
            _logger.LogInformation("This is the playlist route");

            // Get access token
            var accessToken = _sessionService.GetTokenInfo("access_token");

            // Check if access token exists in the session and is not null
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Missing 'access_token' parameter from the session.");

                // Redirect back to Spotify login page
                return Redirect("/auth/login");
            }

            // Get token expiration time
            var strExpiresIn = _sessionService.GetTokenInfo("expires_in");

            if (string.IsNullOrEmpty(strExpiresIn))
            {
                _logger.LogWarning("Missing 'expires_in' parameter in the callback request.");
                return BadRequest("expires_in is null or empty");
            }

            // Set expiresIn
            long expiresIn = _tokenHelper.ParseToLong(strExpiresIn);

            // Check If the token is expired
            if (_tokenHelper.IsExpired(expiresIn))
            {
                _logger.LogWarning("Token expired, need to refresh.");

                // Redirect to refresh token
                return Redirect("/auth/refresh_token");
            }

            string apiBaseUri = _spotifyOptions.ApiBaseUri;
            string endPoint = "me/playlists";

            // Fetch playlists from Spotify
            var response = await _httpService.GetHttpResponseAsync(accessToken, apiBaseUri, endPoint);

            // Handling response error
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Error fetching playlists: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, $"Error fetching playlists: {response.StatusCode}");
            }

            // Getting result as a response
            var result = await response.Content.ReadAsStringAsync();

            try
            {
                // Deserialize playlist
                var playlists = JsonSerializer.Deserialize<JsonElement>(result);

                // Returning the playlists
                return Ok(playlists);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse Spotify playlists response: {Response}", result);
                return StatusCode(500, "Failed to parse playlists response from Spotify.");
            }
        }
    }
}