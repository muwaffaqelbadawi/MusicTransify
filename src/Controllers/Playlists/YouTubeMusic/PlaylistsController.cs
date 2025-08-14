using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Utilities.Token;
using MusicTransify.src.Services.Cache;
using MusicTransify.src.Contracts.Services.Playlists.YouTubeMusic;
using MusicTransify.src.Utilities.Session.YouTubeMusic;

namespace MusicTransify.src.Controllers.Playlists.YouTubeMusic
{
    [ApiController]
    [Route("api/youtube/[controller]")]
    public class PlaylistsController : Controller
    {
        private readonly IYouTubeMusicPlaylistsService _playlistService;
        private readonly YouTubeMusicTokenInfoHelper _tokenInfo;
        private readonly TokenHelper _tokenHelper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<PlaylistsController> _logger;

        public PlaylistsController
        (
            IYouTubeMusicPlaylistsService playlistService,
            YouTubeMusicTokenInfoHelper tokenInfo,
            TokenHelper token,
            ICacheService cacheService,
            ILogger<PlaylistsController> logger
        )
        {
            _playlistService = playlistService;
            _tokenInfo = tokenInfo;
            _tokenHelper = token;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPlaylistsAsync()
        {
            _logger.LogInformation("YouTubeMusic PlaylistsController hit ✅");

            string accessToken = _tokenInfo.AccessToken;

            _logger.LogInformation(
                "Access token restored from session. AccessToken: {AccessToken}", accessToken
            );

            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Missing 'accessToken'. Redirect back to YouTube login page");

                // Redirect back to YouTube login page
                return Redirect("/api/youtube/login");
            }

            // Get token expiration time from session
            string strExpiresIn = _tokenInfo.ExpiresIn;

            if (string.IsNullOrEmpty(strExpiresIn))
            {
                _logger.LogWarning("Missing 'expires_in' from session.");
                return BadRequest("Missing 'expires_in' from session.");
            }

            long expiresIn = _tokenHelper.ParseToLong(strExpiresIn);

            if (_tokenHelper.IsExpired(expiresIn))
            {
                _logger.LogWarning("Token expired, need to refresh.");

                return Redirect("/api/youtube/refreshToken");
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
                var playlists = await _playlistService.GetPlaylistsAsync();

                // Cache the response for 10 minutes
                await _cacheService.SetAsync(cacheKey, playlists, TimeSpan.FromMinutes(10));

                _logger.LogInformation("Playlists cached and returned.");

                // Returning the playlists with handling for empty results
                return playlists?.Items?.Count > 0
                    ? Ok(playlists)
                    : NotFound("No playlists found.");
            }

            catch (JsonException ex)
            {
                _logger.LogError(ex, "An error occurred while fetching playlists from YouTubeMusic.");
                return StatusCode(500, "Internal server error while fetching playlists.");
            }
        }
    }
}
