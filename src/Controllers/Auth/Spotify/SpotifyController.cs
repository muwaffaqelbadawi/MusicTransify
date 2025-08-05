using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Services.Session.Spotify;
using MusicTransify.src.Services.Auth.Spotify;
using MusicTransify.src.Utilities.Token;
using MusicTransify.src.Contracts.DTOs.Request.Shared;

namespace MusicTransify.src.Controllers.Auth.Spotify
{
    [ApiController]
    [Route("api/spotify")]

    public class SpotifyController : Controller
    {
        private readonly SpotifyService _spotifyService;
        private readonly SpotifySessionService _sessionService;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<SpotifyController> _logger;

        public SpotifyController(
            SpotifyService spotifyService,
            SpotifySessionService sessionService,
            TokenHelper tokenHelper,
            ILogger<SpotifyController> logger
        )
        {
            _spotifyService = spotifyService;
            _sessionService = sessionService;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            // Set redirect URI
            string redirectUri = _spotifyService.GetLoginUri();

            _logger.LogInformation("Redirecting to Spotify login: {RedirectUri}", redirectUri);

            return Redirect(redirectUri);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> CallbackAsync([FromQuery] CallbackRequest request)
        {
            _logger.LogInformation("This is the callback route");

            var validationResult = ValidateCallbackRequest(request);
            if (validationResult is not null) return validationResult;

            // Receive the access token
            var accessToken = await _spotifyService.ExchangeAuthorizationCodeAsync(request.Code ?? string.Empty);

            if (accessToken is null)
            {
                _logger.LogError("Failed to exchange authorization code for token.");

                return BadRequest("Failed to exchange authorization code.");
            }

            // Store token assets in session
            _sessionService.Store(accessToken);

            _logger.LogInformation("accessToken successfully stored in session redirecting to playlist route...");

            _logger.LogInformation("Token stored: {accessToken}", accessToken?.AccessToken);


            // Redirect to Spotify playlist controller
            return Redirect("/api/spotify/playlist");

            // Redirect back to playlists
            // return Ok("Access token granted for Spotify Auth access and stored successfully. You can now access your playlists.");
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            _logger.LogInformation("This is the refreshToken route");

            // Set access token
            var accessToken = _sessionService.GetTokenInfo("access_token");

            // Set expiresIn
            var strExpiresIn = _sessionService.GetTokenInfo("expires_in");

            // Set refreshToken
            var refreshToken = _sessionService.GetTokenInfo("refreshToken");

            if (string.IsNullOrEmpty(refreshToken))
            {
                _logger.LogWarning("Refresh token missing from session.");

                // Return the JSON code message if not exists
                return BadRequest("The 'refreshToken' parameter is missing.");
            }

            if (string.IsNullOrEmpty(strExpiresIn))
            {
                _logger.LogWarning("The 'expires_in' parameter is missing.");
                return BadRequest("The 'expires_in' parameter is missing.");
            }

            long expiresIn = _tokenHelper.ParseToLong(strExpiresIn);

            // Check if access_token exists in the session and is not null
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Access token missing from session; redirecting to login.");

                // Redirect back to login route
                return Redirect("/api/spotify/login");
            }

            // Check If the access_token is expired
            if (_tokenHelper.IsExpired(expiresIn))
            {
                if (string.IsNullOrEmpty(refreshToken))
                {
                    _logger.LogWarning("refreshToken value missing from session.");

                    // Return the JSON code message if not exists
                    return BadRequest("The 'refreshToken' parameter is missing.");
                }

                try
                {
                    // Receive the new access token Info
                    var newAccessToken = await _spotifyService.GetNewTokenAsync(refreshToken);

                    if (newAccessToken is null)
                    {
                        _logger.LogError("Failed to get new token using refresh token.");

                        return BadRequest("Failed to get new token");
                    }

                    // Store the new token in session
                    _sessionService.Store(newAccessToken);

                    _logger.LogInformation("Token successfully refreshed and stored in session. Redirecting...");
                }
                catch (JsonException)
                {
                    throw new JsonException("Failed to receive Spotify new access token");
                }

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

            _logger.LogInformation("No scope returned; assuming all requested scopes were granted.");


            if (!string.IsNullOrEmpty(request.Error))
            {
                _logger.LogWarning("OAuth error in callback: {Error}", request.Error);
            }

            return null!;
        }
    }
}