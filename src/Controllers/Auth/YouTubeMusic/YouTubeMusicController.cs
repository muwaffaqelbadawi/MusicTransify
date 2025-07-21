using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Models.YouTubeMusic;
using MusicTransify.src.Services.Auth.YouTubeMusic;
using MusicTransify.src.Services.Session;
using MusicTransify.src.Utilities.Token;

namespace MusicTransify.src.Controllers.Auth.YouTubeMusic
{
    [ApiController]
    [Route("youtube")] // Route /youtube
    public class YouTubeMusicController : Controller
    {
        private readonly YouTubeMusicService _youTubeMusicAuthService;
        private readonly SessionService _sessionService;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<YouTubeMusicController> _logger;
        public YouTubeMusicController(
            YouTubeMusicService youTubeMusicAuthService,
            SessionService sessionService,
            TokenHelper tokenHelper,
            ILogger<YouTubeMusicController> logger
        )
        {
            _youTubeMusicAuthService = youTubeMusicAuthService;
            _sessionService = sessionService;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        [HttpGet("login")] // Route: "/youtube/login"
        public IActionResult Login()
        {
            _logger.LogInformation("This is the Login route");

            // Set redirect URI
            string redirectUri = _youTubeMusicAuthService.GetLoginUri();

            return Redirect(redirectUri);
        }

        [HttpGet("callback")] // Route: "/youtube/callback"
        public async Task<IActionResult> CallbackAsync([FromQuery] Callback request)
        {
            _logger.LogInformation("This is the callback route");

            // Check if "error" exists in the query string and not null
            if (!string.IsNullOrEmpty(request.Error))
            {
                _logger.LogWarning("OAuth callback error: {Error}", request.Error);

                // Return the JSON error message if not exists
                return BadRequest(new { request.Error });
            }

            // Check if "code" does not exists in the query string and not null
            if (string.IsNullOrEmpty(request.Code))
            {
                _logger.LogWarning("Missing 'code' parameter in the callback request.");

                // Return the JSON code message if not exists
                return BadRequest("Missing 'code' parameter in the callback request.");
            }

            // Receive the Token Info
            var tokenInfo = await _youTubeMusicAuthService.ExchangeAuthorizationCodeAsync(request.Code);

            // Redirect
            return Redirect("/youtube/login");
        }

        [HttpGet("refreshToken")] // Route: "/refresh_token"
        public async Task<IActionResult> RefreshTokenAsync()
        {
            _logger.LogInformation("This is the refreshToken route");

            // Set access token
            var accessToken = _sessionService.GetTokenInfo("access_token");

            // Set expiresIn
            var strExpiresIn = _sessionService.GetTokenInfo("expires_in");

            // Set refresh_token
            var refreshToken = _sessionService.GetTokenInfo("refresh_token");

            if (string.IsNullOrEmpty(strExpiresIn))
            {
                _logger.LogWarning("The 'expires_in' parameter is missing or invalid in the session.");
                return BadRequest("The 'expires_in' parameter is missing or invalid in the session.");
            }

            long expiresIn = _tokenHelper.ParseToLong(strExpiresIn);

            // Check if access_token exists in the session and is not null
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Access token missing from session; redirecting to login.");
                // Redirect back to login route
                return Redirect("login");
            }

            // Check If the access_token is expired
            if (_tokenHelper.IsExpired(expiresIn))
            {
                // Check if refresh token does not exist or invalid in the session
                if (string.IsNullOrEmpty(refreshToken))
                {
                    _logger.LogWarning("Refresh token missing from session.");

                    // Return the JSON code message if not exists
                    return BadRequest("The 'refresh_token' parameter is missing or invalid in the session.");
                }

                // Receive the Token Info
                var TokenInfo = await _youTubeMusicAuthService.GetNewTokenAsync(refreshToken);

                if (TokenInfo.ValueKind == JsonValueKind.Undefined)
                {
                    _logger.LogError("Failed to get new token using refresh token.");

                    return BadRequest("Failed to get new token");
                }

                // Store token info in session
                _sessionService.Store(TokenInfo);

                _logger.LogInformation("Token successfully refreshed and stored in session. Redirecting...");

                // Redirect
                return Redirect("/youtube/playlist");
            }

            // Redirect
            return Redirect("/youtube/refreshToken");
        }
    }
}