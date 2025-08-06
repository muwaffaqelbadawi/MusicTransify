using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Utilities.Token;
using MusicTransify.src.Services.Cache;
using MusicTransify.src.Contracts.Services.Playlist.Spotify;
using MusicTransify.src.Contracts.DTOs.Response.Playlist.Spotify;
using MusicTransify.src.Contracts.Session.Spotify;

namespace MusicTransify.src.Controllers.Playlists.Spotify
{
    [ApiController]
    [Route("api/spotify/[controller]")]
    public class PlaylistController : Controller
    {
        private readonly ISpotifyPlaylistService _spotifyPlaylistService;
        private readonly ISpotifySessionService _sessionService;
        private readonly TokenHelper _tokenHelper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<PlaylistController> _logger;

        public PlaylistController(
            ISpotifyPlaylistService spotifyPlaylistService,
            ISpotifySessionService sessionService,
            TokenHelper token,
            ICacheService cacheService,
            ILogger<PlaylistController> logger
        )
        {
            _spotifyPlaylistService = spotifyPlaylistService;
            _sessionService = sessionService;
            _tokenHelper = token;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPlaylistsAsync()
        {
            _logger.LogInformation("Spotify PlaylistController hit ✅");


            // Get access token
            string accessToken = _sessionService.GetTokenInfo("access_token") ?? string.Empty;

            // Check if access token exists in the session and is not null
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Missing 'accessToken' parameter from the session.");

                // Redirect back to Spotify login page
                return Redirect("/api/spotify/login");
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
                return Redirect("/api/spotify/refreshToken");
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

                if (playlist?.Items is null)
                {
                    _logger.LogError("No playlists found.");
                    return StatusCode(500, "Playlist not found");
                }

                // Cache the response for 10 minutes
                await _cacheService.SetAsync(cacheKey, playlist, TimeSpan.FromMinutes(10));

                _logger.LogInformation("Playlists cached and returned.");

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