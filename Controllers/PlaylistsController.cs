using System;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using SpotifyWebAPI_Intro.Services;
using SpotifyWebAPI_Intro.utilities;

namespace SpotifyWebAPI_Intro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("playlists")]
    public class PlaylistsController : ControllerBase
    {
        private readonly OptionsService _optionsService;
        private readonly SessionService _sessionService;
        private readonly AuthHelper _authHelper;

        public PlaylistsController(OptionsService optionsService, SessionService sessionService, AuthHelper authHelper)
        {
            _optionsService = optionsService;
            _sessionService = sessionService;
            _authHelper = authHelper;
        }

        [HttpGet("Playlists")]
        public async Task<IActionResult> GetPlaylists()
        {
            var AccessToken = _sessionService.RevealAssete("AccessToken");

            // Set APIBase URI
            string APIBaseURL = _optionsService.SpotifyApiBaseUrl;

            // Check if access_token exists in the session and is not null
            if (string.IsNullOrEmpty(AccessToken))
            {
                // Redirect back to Spotify login page
                Redirect("/login");

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
                Redirect("/refresh_token");

                // Successful redirection to auth_url
                return Ok("Redirect to refresh token");
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

            // Getting the playlists response back
            await Response.WriteAsJsonAsync(playlists);

            // Successfully redirecting to playlists route
            return Ok("playlists route");
        }
    }
}