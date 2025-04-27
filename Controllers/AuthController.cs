using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SpotifyWebAPI_Intro.Models;
using SpotifyWebAPI_Intro.Services;
using SpotifyWebAPI_Intro.utilities;

namespace SpotifyWebAPI_Intro.Controllers
{
    [ApiController]
    [Route("auth")] // Base route "/auth"
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly SessionService _sessionService;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, SessionService sessionService, TokenHelper tokenHelper, ILogger<AuthController> logger)
        {
            _authService = authService;
            _sessionService = sessionService;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        [HttpGet("login")] // Route: "/auth/login"
        public IActionResult Login()
        {
            // Use the log information
            _logger.LogInformation("This is the LogIn route");

            string redirectUri = _authService.GetLogInURI();
            return Redirect(redirectUri);
        }

        [HttpGet("Callback")] // Route: "/auth/callback"
        public async Task<IActionResult> Callback([FromQuery] CallbackRequest request)
        {
            // Use the log information
            _logger.LogInformation("This is the Callback roue");

            // Check if "error" exists in the query string and not null
            if (!string.IsNullOrEmpty(request.Error))
            {
                // Return the JSON error message if not exists
                return BadRequest(new { request.Error });
            }

            // Check if "code" does not exists in the query string and not null
            if (string.IsNullOrEmpty(request.Code))
            {
                // Return the JSON code message if not exists
                return BadRequest("Missing 'code' parameter in the callback request.");
            }

            // Receive the Token Info
            var TokenInfo = await _authService.ExchangeAuthorizationCodeAsync(request.Code);

            if (TokenInfo.ValueKind == JsonValueKind.Undefined)
            {
                return BadRequest("Failed to exchange authorization code.");
            }

            // Store token assets in session
            _sessionService.Store(TokenInfo);

            // Redirect back to playlists
            return Redirect("/playlists");
        }

        [HttpGet("refresh_token")] // Route: "/auth/refresh_token"
        public async Task<IActionResult> RefreshToken([FromQuery] CallbackRequest request)
        {
            // Use the log information
            _logger.LogInformation("This is the Refresh Token route");

            var AccessToken = _sessionService.GetTokenInfo("AccessToken");

            // Set and Check if access_token exists in the session and is not null
            if (string.IsNullOrEmpty(AccessToken))
            {
                // The access token is not exist
                Redirect("/login");

                // Redirect back to Spotify login route
                return BadRequest("Redirect to login route");
            }

            // Check If the access_token is expired
            if (_tokenHelper.IsExpired(_sessionService.GetTokenInfo("ExpiresIn")))
            {
                // Check if "refresh_token" does not exists in the query string and not null
                if (string.IsNullOrEmpty(request.RefreshToken))
                {
                    // Return the JSON code message if not exists
                    return BadRequest("The 'refresh_token' parameter is missing in the query string.");
                }

                // Receive the Token Info
                var TokenInfo = await _authService.GetNewTokenAsync(request.RefreshToken);

                if (TokenInfo.ValueKind == JsonValueKind.Undefined)
                {
                    return BadRequest("Failed to get new token");
                }

                // Store token assets
                _sessionService.Store(TokenInfo);

                // Redirect back to playlists route
                return Redirect("/playlists");
            }

            // Redirect back to playlists route
            return Redirect("/playlists");
        }
    }
}