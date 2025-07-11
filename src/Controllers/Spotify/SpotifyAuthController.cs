using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Controllers.Common;
using MusicTransify.src.Models.Spotify;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Utilities.Common;

namespace MusicTransify.src.Controllers.Spotify
{
    [ApiController]
    [Route("auth/spotify")] // route "/auth/spotify"

    public class SpotifyAuthController : AuthController
    {
        private readonly TokenHelper _tokenHelper;
        public SpotifyAuthController(
            AuthService authService,
            SessionService sessionService,
            TokenHelper tokenHelper,
            ILogger<AuthController> logger)
            : base(authService, sessionService, logger)
        {
            _tokenHelper = tokenHelper;
        }

        [HttpGet("login/spotify")] // Route: "/auth/login/spotify"
        public IActionResult Login()
        {
            _logger.LogInformation("This is the Login route");

            // Set redirect URI
            string redirectUri = _authService.GetLogInURI();

            return Redirect(redirectUri);
        }

        [HttpGet("callback")] // Route: "/auth/callback"
        public async Task<IActionResult> CallbackAsync([FromQuery] SpotifyCallback request)
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
            var tokenInfo = await _authService.ExchangeAuthorizationCodeAsync(request.Code);

            if (tokenInfo.ValueKind == JsonValueKind.Undefined)
            {
                _logger.LogError("Failed to exchange authorization code for token.");

                return BadRequest("Failed to exchange authorization code.");
            }

            // Store token assets in session
            _sessionService.Store(tokenInfo);

            _logger.LogInformation("tokenInfo successfully stored in session. Redirecting to /playlists.");

            // Redirect back to playlists
            return Redirect("/playlists");
        }

        [HttpGet("refresh_token")] // Route: "/auth/refresh_token"
        public async Task<IActionResult> RefreshTokenAsync()
        {
            _logger.LogInformation("This is the refresh_token route");

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
                return Redirect("/auth/login");
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
                var TokenInfo = await _authService.GetNewTokenAsync(refreshToken);

                if (TokenInfo.ValueKind == JsonValueKind.Undefined)
                {
                    _logger.LogError("Failed to get new token using refresh token.");

                    return BadRequest("Failed to get new token");
                }

                // Store token info in session
                _sessionService.Store(TokenInfo);

                _logger.LogInformation("Token successfully refreshed and stored in session. Redirecting...");

                // Redirect
                return Redirect("");
            }

            // Redirect
            return Redirect("");
        }
    }
}