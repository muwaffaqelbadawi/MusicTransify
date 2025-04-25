using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SpotifyWebAPI_Intro.Services;
using SpotifyWebAPI_Intro.utilities;

namespace SpotifyWebAPI_Intro.Controllers
{
    [ApiController]
    [Route("playlists")] // Route: "/playlists"
    public class PlaylistsController : ControllerBase
    {
        private readonly SessionService _sessionService;
        private readonly HttpService _httpService;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<PlaylistsController> _logger;

        public PlaylistsController(SessionService sessionService, HttpService httpService, TokenHelper tokenHelper, ILogger<PlaylistsController> logger)
        {
            _sessionService = sessionService;
            _httpService = httpService;
            _tokenHelper = tokenHelper;

            _logger = logger;
        }

        [HttpGet] // Route: "/playlists"
        public async Task<IActionResult> GetPlaylists()
        {
            // Use the log information
            _logger.LogInformation("This is the playlist page");

            // Check if access token exists in the session and is not null
            if (string.IsNullOrEmpty(_sessionService.GetTokenInfo("AccessToken")))
            {
                // Redirect back to Spotify login page
                Redirect("/auth/login");

                // Redirect back to loging
                return BadRequest("Redirect back to login");
            }

            // Check If the access_token is expired
            if (_tokenHelper.IsExpired())
            {
                // Redirect to refresh token
                return Redirect("/auth/refresh_token");
            }

            // Get playlists info
            var response = await _httpService.GetHttpResponseAsync("me/playlists");

            // Handling response error
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching playlists: {response.StatusCode}");
            }

            // Getting result as a response
            var result = await response.Content.ReadAsStringAsync();

            // Deserialize playlist
            var playlists = JsonSerializer.Deserialize<JsonElement>(result);

            // Returning the playlists
            return Ok(playlists);
        }
    }
}