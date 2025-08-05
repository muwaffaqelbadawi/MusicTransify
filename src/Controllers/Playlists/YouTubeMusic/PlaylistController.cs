using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Utilities.Token;
using MusicTransify.src.Services.Cache;
using MusicTransify.src.Contracts.Services.Playlist.YouTubeMusic;
using MusicTransify.src.Contracts.DTOs.Response.Playlist.YouTubeMusic;
using MusicTransify.src.Contracts.Session.YouTubeMusic;

namespace MusicTransify.src.Controllers.Playlists.YouTubeMusic
{
    [ApiController]
    [Route("api/youtube/[controller]")]
    public class PlaylistController : Controller
    {
        private readonly IYouTubeMusicPlaylistService _playlistService;
        private readonly IYouTubeMusicSessionService _sessionService;
        private readonly TokenHelper _tokenHelper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<PlaylistController> _logger;

        public PlaylistController
        (
            IYouTubeMusicPlaylistService playlistService,
            IYouTubeMusicSessionService sessionService,
            TokenHelper token,
            ICacheService cacheService,
            ILogger<PlaylistController> logger

        )
        {
            _playlistService = playlistService;
            _sessionService = sessionService;
            _tokenHelper = token;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPlaylistsAsync()
        {
            _logger.LogInformation("YouTubeMusic PlaylistController hit ✅");

            // Get access token
            string accessToken = _sessionService.GetTokenInfo("access_token") ?? string.Empty;

            // Check if access token exists in the session and is not null
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Missing 'accessToken' parameter from the session.");

                // Redirect back to YouTube Music login page
                return Redirect("/youtube/login");
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
                return Redirect("/youtube/refreshToken");
            }

            // Caching
            var cacheKey = $"YouTubeMusic:User:{accessToken}:Playlists";
            var cachedPlaylists = await _cacheService.GetAsync<JsonElement>(cacheKey);

            if (cachedPlaylists.ValueKind != JsonValueKind.Undefined)
            {
                _logger.LogInformation("Serving playlists from cache.");
                return Ok(cachedPlaylists);
            }

            try
            {
                // Deserialize playlist
                var playlist = await _playlistService.GetPlaylistAsync<YouTubeMusicPlaylistResponseWrapper>();

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
                _logger.LogError(ex, "An error occurred while fetching playlists from YouTubeMusic.");
                return StatusCode(500, "Internal server error while fetching playlists.");
            }
        }
    }
}
