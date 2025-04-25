using System;
using System.Text;
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
        private readonly OptionsService _optionsService;
        private readonly SessionService _sessionService;
        private readonly HttpService _httpService;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, OptionsService optionsService, SessionService sessionService, HttpService httpService, TokenHelper tokenHelper, ILogger<AuthController> logger)
        {
            _authService = authService;
            _optionsService = optionsService;
            _sessionService = sessionService;
            _httpService = httpService;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        [HttpGet("login")] // Route: "/auth/login"
        public IActionResult Login()
        {
            // Use the log information
            _logger.LogInformation("This is the loging page");

            string redirectUrl = _authService.GetLogInURL();
            return Redirect(redirectUrl);
        }

        [HttpGet("Callback")] // Route: "/auth/callback"
        public async Task<IActionResult> Callback([FromQuery] CallbackRequest request)
        {
            // Use the log information
            _logger.LogInformation("This is the callback page");

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
            _logger.LogInformation("This is the refresh token page");

            var AccessToken = _sessionService.GetTokenInfo("AccessToken");

            // Set and Check if access_token exists in the session and is not null
            if (string.IsNullOrEmpty(AccessToken))
            {
                // The access token is not exist
                Redirect("/login");

                // Redirect back to Spotify login page
                return BadRequest("Redirect to login route");
            }

            // Check If the access_token is expired
            if (_tokenHelper.IsExpired())
            {
                // Set the grant_type
                string GrantType = "refresh_token";

                // Check if "refresh_token" does not exists in the query string and not null
                if (string.IsNullOrEmpty(request.RefreshToken))
                {
                    // Return the JSON code message if not exists
                    return BadRequest("The 'refresh_token' parameter is missing in the query string.");
                }

                // Set Client ID
                string ClientID = _optionsService.SpotifyClientId;

                // Set Client Secret
                string ClientSecret = _optionsService.SpotifyClientSecret;

                // Set Token URL
                string TokenURL = _optionsService.SpotifyTokenUrl;

                // Initialize request body
                var RequestBody = new Dictionary<string, string>
                {
                  { "grant_type", GrantType },
                  { "refresh_token", request.RefreshToken },
                  { "client_id", ClientID },
                  { "client_secret", ClientSecret }
                };

                // Set Token Info
                var TokenInfo = await _httpService.PostFormUrlEncodedContentAsync(TokenURL, RequestBody);

                // Check existence of Token Assets and return thier values
                var Assets = _sessionService.Check(TokenInfo);

                // Calculate refresh token expiration date
                string ExpiresIn = _tokenHelper.CalculateExpirationDate(Assets.ExpiresIn);

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