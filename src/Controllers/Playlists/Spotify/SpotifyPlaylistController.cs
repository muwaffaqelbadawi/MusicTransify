using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Utilities.Token;
using MusicTransify.src.Services.Cache;
using MusicTransify.src.Contracts.Services.Playlist.Spotify;
using MusicTransify.src.Contracts.DTOs.Response.Playlist.Spotify;
using MusicTransify.src.Services.Session.Spotify;

namespace MusicTransify.src.Controllers.Playlists.Spotify
{
    [ApiController]
    [Route("/spotify/playlist")] // Route: "/spotify/playlist"
    public class SpotifyPlaylistController : Controller
    {
        private readonly ISpotifyPlaylistService _spotifyPlaylistService;
        private readonly SpotifySessionService _sessionService;
        private readonly TokenHelper _tokenHelper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<SpotifyPlaylistController> _logger;

        public SpotifyPlaylistController(
            ISpotifyPlaylistService spotifyPlaylistService,
            SpotifySessionService sessionService,
            TokenHelper token,
            ICacheService cacheService,
            ILogger<SpotifyPlaylistController> logger
        )
        {
            _spotifyPlaylistService = spotifyPlaylistService;
            _sessionService = sessionService;
            _tokenHelper = token;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet("")] // Route: "/spotify/playlist"
        public async Task<IActionResult> GetPlaylistsAsync()
        {
            // Use the log information
            _logger.LogInformation("Playlist controller accessed");

            // Get access token
            string accessToken = _sessionService.GetTokenInfo("access_token") ?? string.Empty;

            // Check if access token exists in the session and is not null
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Missing 'accessToken' parameter from the session.");

                // Redirect back to Spotify login page
                return Redirect("/spotify/login");
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
                return Redirect("/spotify/refresh_token"); // route "/spotify/refresh_token"
            }

            // Caching
            var cacheKey = $"Spotify:User:{accessToken}:Playlists";
            var cachedPlaylists = await _cacheService.GetAsync<JsonElement>(cacheKey);

            if (cachedPlaylists.ValueKind != JsonValueKind.Undefined)
            {
                _logger.LogInformation("Serving playlists from cache.");
                return Ok(cachedPlaylists);
            }

            try
            {
                // Deserialize playlist
                var playlist = await _spotifyPlaylistService.GetPlaylistAsync<SpotifyPlaylistsResponseWrapper>();

                if (playlist is null)
                {
                    _logger.LogError("Failed to deserialize playlist response.");
                    return StatusCode(500, "Playlist not found");
                }

                if (playlist is null || playlist.Items is null)
                {
                    _logger.LogError("Failed to fetch playlists or received empty result.");
                    return StatusCode(500, "Internal server error");
                }

                // Otherwise, return the actual playlists
                _logger.LogInformation("Playlists fetched successfully. Caching result...");
                await _cacheService.SetAsync(cacheKey, playlist, TimeSpan.FromMinutes(10));

                _logger.LogInformation("Playlists response cached successfully.");
                
                // Cache the response for 10 minutes
                await _cacheService.SetAsync(cacheKey, playlist, TimeSpan.FromMinutes(10));

                // Returning the playlists
                return Ok(playlist);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "An error occurred while fetching playlists from Spotify.");
                return StatusCode(500, "Internal server error while fetching playlists.");
            }
        }
    }
}