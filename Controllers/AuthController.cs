using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using SpotifyWebAPI_Intro.Models;
using SpotifyWebAPI_Intro.Services;
using SpotifyWebAPI_Intro.utilities;

namespace SpotifyWebAPI_Intro.Controllers
{
    [ApiController]
    [Route("auth")] // Base route "/auth"
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthService _authService;
        private readonly OptionsService _optionsService;
        private readonly SessionService _sessionService;
        private readonly HttpService _httpService;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IHttpContextAccessor httpContextAccessor, AuthService authService, OptionsService optionsService, SessionService sessionService, HttpService httpService, AuthHelper authHelper, ILogger<AuthController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _optionsService = optionsService;
            _sessionService = sessionService;
            _httpService = httpService;
            _authHelper = authHelper;
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

            // Recieve the Token Info
            var TokenInfo = await _authService.ExchangeAuthorizationCodeAsync(request.Code ?? throw new InvalidCastException("Code parameter is not found"));

            // Check existence of token assets
            var Assets = _sessionService.CheckAssets(TokenInfo);

            // Calculate token expiration date
            string ExpiresIn = _authHelper.CalculateTokenExpirationDate(Assets.ExpiresIn);

            // Store token assets in session
            _sessionService.StoreAssetes(Assets.AccessToken, Assets.RefreshToken, ExpiresIn);

            // Redirect back to playlists
            return Redirect("/playlists");
        }

        [HttpGet("refresh_token")] // Route: "/auth/refresh_token"
        public async Task<IActionResult> RefreshToken()
        {
            // Use the log information
            _logger.LogInformation("This is the refresh token page");


            var AccessToken = _sessionService.RevealAssete("AccessToken");
            var OldExpiresIn = _sessionService.RevealAssete("ExpiresIn");

            // Set and Check if access_token exists in the session and is not null
            if (string.IsNullOrEmpty(AccessToken))
            {
                // The access token is not exist
                Redirect("/login");

                // Redirect back to Spotify login page
                return BadRequest("Redirect to login route");
            }

            // Check If the access_token is expired
            if (_authHelper.IsExpired(OldExpiresIn))
            {
                // Console prompt for debugging
                Console.WriteLine("TOKEN EXPIRED. REFRESHING...");

                // Set the grant_type
                string GrantType = "refresh_token";

                // Check refresh_token value exists in query string and is not null
                string OldRefreshToken = _httpContextAccessor.HttpContext?.Request.Query["refresh_token"].ToString() ?? throw new InvalidOperationException("The 'refresh_token' parameter is missing in the query string.");

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
                  { "refresh_token", OldRefreshToken },
                  { "client_id", ClientID },
                  { "client_secret", ClientSecret }
                };

                // Set Token Info
                var TokenInfo = await _httpService.PostFormUrlEncodedContentAsync(TokenURL, RequestBody);

                // Check existence of Token Assets and return thier values
                var Assets = _sessionService.CheckAssets(TokenInfo);

                // Calculate refresh token expiration date
                string ExpiresIn = _authHelper.CalculateTokenExpirationDate(Assets.ExpiresIn);

                // Store token assets
                _sessionService.StoreAssetes(Assets.AccessToken, Assets.RefreshToken, ExpiresIn);

                // Redirect back to playlists route
                return Redirect("/playlists");
            }

            // Redirect back to playlists route
            return Redirect("/playlists");
        }
    }
}