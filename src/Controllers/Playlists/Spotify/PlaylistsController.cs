using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Api.Spotify.Playlists.Responses;
using MusicTransify.src.Contracts.Services.Playlists.Spotify;
using MusicTransify.src.Services.Cache;
using MusicTransify.src.Utilities.Session.Spotify;
using MusicTransify.src.Utilities.Token;

namespace MusicTransify.src.Controllers.Playlists.Spotify
{
    [ApiController]
    [Route("api/spotify/[controller]")]
    public class PlaylistController : Controller
    {
        private readonly ISpotifyPlaylistsService _playlistsService;
        private readonly SpotifyTokenInfoHelper _tokenInfo;
        private readonly TokenHelper _tokenHelper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<PlaylistController> _logger;

        public PlaylistController(
            ISpotifyPlaylistsService playlistsService,
            SpotifyTokenInfoHelper tokenInfo,
            TokenHelper token,
            ICacheService cacheService,
            ILogger<PlaylistController> logger
        )
        {
            _playlistsService = playlistsService;
            _tokenInfo = tokenInfo;
            _tokenHelper = token;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPlaylistsAsync()
        {
            _logger.LogInformation("Spotify PlaylistController hit ✅");

            // GET the access token stored in the session
            string accessToken = _tokenInfo.AccessToken;

            _logger.LogInformation(
                "Access token restored from session. AccessToken: {AccessToken}", accessToken
            );

            if (string.IsNullOrEmpty(accessToken))
            {
                // Here's the problem (Spotify PlaylistsController)
                _logger.LogWarning("Missing 'accessToken' parameter from the session. Redirecting to login from Spotify PlaylistsController...");

                // Redirect back to Spotify login page
                return Redirect("/api/spotify/login");
            }

            // Get token expiration time
            string strExpiresIn = _tokenInfo.ExpiresIn;

            if (string.IsNullOrEmpty(strExpiresIn))
            {
                _logger.LogWarning("Missing 'expires_in' from session.");
                return BadRequest("Missing 'expires_in' from session.");
            }

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
                var playlist = await _playlistsService.GetPlaylistAsync<SpotifyPlaylistsResponseWrapperDto>();

                if (playlist?.Items is null)
                {
                    _logger.LogError("No playlists found.");
                    return StatusCode(500, "Playlist not found");
                }

                // Cache the response for 10 minutes
                await _cacheService.SetAsync(cacheKey, playlist, TimeSpan.FromMinutes(10));

                _logger.LogInformation("Playlists cached and returned.");

                // GET the playlists
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