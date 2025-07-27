using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Contracts.DTOs;
using MusicTransify.src.Services.Session;
using MusicTransify.src.Services.Auth.Spotify;
using MusicTransify.src.Utilities.Token;

namespace MusicTransify.src.Controllers.Auth.Spotify
{
    [ApiController]
    [Route("spotify")] // Route "/spotify"

    public class SpotifyController : Controller
    {
        private readonly SpotifyService _spotifyService;
        private readonly SessionService _sessionService;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<SpotifyController> _logger;

        public SpotifyController(
            SpotifyService spotifyService,
            SessionService sessionService,
            TokenHelper tokenHelper,
            ILogger<SpotifyController> logger
        )
        {
            _spotifyService = spotifyService;
            _sessionService = sessionService;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        [HttpGet("login")] // Route "/spotify/login"
        public IActionResult Login()
        {
            // Set redirect URI
            string redirectUri = _spotifyService.GetLoginUri();

            _logger.LogInformation("Redirecting to Spotify login: {RedirectUri}", redirectUri);

            return Redirect(redirectUri);
        }

        [HttpGet("callback")] // Route: "/spotify/callback"
        public async Task<IActionResult> CallbackAsync([FromQuery] CallbackRequest request)
        {
            _logger.LogInformation("This is the callback route");

            var validationResult = ValidateCallbackRequest(request);
            if (validationResult is not null) return validationResult;

            // Receive the access token
            var accessToken = await _spotifyService.ExchangeAuthorizationCodeAsync(request.Code ?? string.Empty);

            if (accessToken.ValueKind == JsonValueKind.Undefined)
            {
                _logger.LogError("Failed to exchange authorization code for token.");

                return BadRequest("Failed to exchange authorization code.");
            }

            // Store token assets in session
            _sessionService.Store(accessToken);

            _logger.LogInformation("accessToken successfully stored in session redirecting...");

            // Redirect back to playlists
            // return Redirect("/spotify/playlist");

            // Redirect back to playlists
            return Ok("Access token granted for Spotify Auth access and stored successfully. You can now access your playlists.");
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
                return Redirect("/spotify/login");
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

                // Receive the new access token Info
                var newAccessToken = await _spotifyService.GetNewTokenAsync(refreshToken);

                if (newAccessToken.ValueKind == JsonValueKind.Undefined)
                {
                    _logger.LogError("Failed to get new token using refresh token.");

                    return BadRequest("Failed to get new token");
                }

                // Store the new token in session
                _sessionService.Store(newAccessToken);

                _logger.LogInformation("Token successfully refreshed and stored in session. Redirecting...");

                // Successfully refreshed token, redirect back to playlists
                // return Redirect("/spotify/playlist");
                return Ok("Access token granted for Spotify Auth access and stored successfully. You can now access your playlists.");
            }

            _logger.LogInformation("Token is still valid, no refresh needed.");

            // Redirect
            return Ok("Token is still valid, no refresh needed.");
        }

        private BadRequestObjectResult? ValidateCallbackRequest(CallbackRequest request)
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                _logger.LogWarning("Missing 'Code' parameter in callback request.");
                return BadRequest("Missing 'Code' parameter in callback request.");
            }

            if (string.IsNullOrEmpty(request.State))
            {
                _logger.LogWarning("Missing 'State' parameter in callback (optional).");
                return BadRequest("Missing 'State' parameter in callback request.");
            }

            if (string.IsNullOrEmpty(request.Scope))
            {
                _logger.LogWarning("Missing 'Scope' parameter in callback request.");
            }

            if (!string.IsNullOrEmpty(request.Error))
            {
                _logger.LogWarning("OAuth error in callback: {Error}", request.Error);
            }

            return null!;
        }
    }
}