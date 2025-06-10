using System;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Controllers.Common;
using MusicTransify.src.Models.YouTubeMusic;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Services.YouTubeMusic;

namespace MusicTransify.src.Controllers.YouTubeMusic
{
    [ApiController]
    [Route("auth/music.youtube")] // route "/auth/music.youtube"
    public class YouTubeMusicAuthController : AuthController
    {
        public YouTubeMusicAuthController(
            YouTubeMusicAuthService youTubeMusicAuthService,
            SessionService sessionService,
            ILogger<AuthController> logger)
            : base(youTubeMusicAuthService, sessionService, logger)
        {
        }

        [HttpGet("login/music.youtube")] // Route: "/auth/login/music.youtube"
        public IActionResult Login()
        {
            _logger.LogInformation("This is the Login route");

            // Set redirect URI
            string redirectUri = _authService.GetLogInURI();

            return Redirect(redirectUri);
        }

        [HttpGet("callback")] // Route: "/auth/callback"
        public async Task<IActionResult> CallbackAsync([FromQuery] YouTubeMusicCallback request)
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

            // Redirect
            return Redirect("");
        }
    }
}