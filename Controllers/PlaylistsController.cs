using System;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using SpotifyWebAPI_Intro.Services;
using SpotifyWebAPI_Intro.utilities;
using Microsoft.Extensions.Logging;

namespace SpotifyWebAPI_Intro.Controllers
{
    [ApiController]
    [Route("playlists")] // Route: "/playlists"
    public class PlaylistsController : ControllerBase
    {
        private readonly OptionsService _optionsService;
        private readonly SessionService _sessionService;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<PlaylistsController> _logger;

        public PlaylistsController(OptionsService optionsService, SessionService sessionService, AuthHelper authHelper, ILogger<PlaylistsController> logger)
        {
            _optionsService = optionsService;
            _sessionService = sessionService;
            _authHelper = authHelper;
            _logger = logger;
        }

        [HttpGet] // Route: "/playlists"
        public async Task<IActionResult> GetPlaylists()
        {
            // Use the log information
            _logger.LogInformation("This is the playlist page");


            var AccessToken = _sessionService.RevealAssete("AccessToken");

            // Set APIBase URI
            string APIBaseURL = _optionsService.SpotifyApiBaseUrl;

            // Check if access_token exists in the session and is not null
            if (string.IsNullOrEmpty(AccessToken))
            {
                // Redirect back to Spotify login page
                Redirect("/auth/login");

                // Redirect back to loging
                return BadRequest("Redirect back to login");
            }

            var ExpiresIn = _sessionService.RevealAssete("ExpiresIn");

            // Check If the access_token is expired
            if (_authHelper.IsExpired(ExpiresIn))
            {
                // Console prompt for debugging
                Console.WriteLine("TOKEN EXPIRED. REFRESHING...");

                // Redirect to refresh token
                return Redirect("/auth/refresh_token");
            }

            // Create Autorization String
            string Authorization = $"Bearer {AccessToken}";

            //Initiate new http class
            using var client = new HttpClient();

            // Authorization Header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Authorization);

            // Post Form Url Encoded Content
            var response = await client.GetAsync($"{APIBaseURL}me/playlists");

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