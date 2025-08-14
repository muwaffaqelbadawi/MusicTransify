using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Utilities.Token;
using MusicTransify.src.Contracts.Session.Spotify;
using MusicTransify.src.Contracts.Services.Auth.Spotify;
using MusicTransify.src.Api.Spotify.Callback.Responses;

namespace MusicTransify.src.Controllers.Auth.Spotify
{
    [ApiController]
    [Route("api/spotify")]

    public class SpotifyController : Controller
    {
        private readonly ISpotifyService _spotifyService;
        private readonly ISpotifySessionService _sessionService;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<SpotifyController> _logger;

        public SpotifyController(
            ISpotifyService spotifyService,
            ISpotifySessionService sessionService,
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
        public async Task<IActionResult> CallbackAsync([FromQuery] SpotifyCallbackResponseDto response)
        {
            _logger.LogInformation("This is the callback route");

            var validationResult = ValidateCallbackRequest(response);
            if (validationResult is not null) return validationResult;

            if (string.IsNullOrEmpty(response.Code))
            {
                _logger.LogWarning("Authorization code is missing.");
                return BadRequest("Authorization code is missing.");
            }

            // Receive the token info
            var tokenInfo = await _spotifyService.ExchangeAuthorizationCodeAsync(response.Code);

            // Logging the received access token for debugging purposes
            _logger.LogInformation("Token info received successfully");

            if (tokenInfo is null)
            {
                _logger.LogError("Failed to exchange authorization code and getting the token info.");

                return BadRequest("Failed to exchange authorization code and getting the token info.");
            }

            _logger.LogInformation("Token info successfully received.");

            // Store token info in session
            _sessionService.Store(tokenInfo);

            _logger.LogInformation("accessToken successfully stored in session redirecting to playlist route...");

            // This must be changed to a resolved URL
            return Redirect("http://localhost:3000/playlists/spotify");
            // return Redirect("/api/spotify/login");
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
                // Here's the problem (SpotifyController)
                _logger.LogWarning("Missing 'accessToken' parameter from the session. Redirecting to login from SpotifyController...");

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

        private BadRequestObjectResult? ValidateCallbackRequest(SpotifyCallbackResponseDto response)
        {
            if (string.IsNullOrEmpty(response.Code))
            {
                _logger.LogWarning("Missing 'Code' parameter in callback request.");
                return BadRequest("Missing 'Code' parameter in callback request.");
            }

            if (string.IsNullOrEmpty(response.State))
            {
                _logger.LogWarning("Missing 'State' parameter in callback (optional).");
                return BadRequest("Missing 'State' parameter in callback request.");
            }

            _logger.LogInformation("No scope returned; assuming all requested scopes were granted.");

            if (!string.IsNullOrEmpty(response.Error))
            {
                _logger.LogWarning("OAuth error in callback: {Error}", response.Error);
            }

            return null!;
        }
    }
}