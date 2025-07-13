using System;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Contracts;
using MusicTransify.src.Models.YouTubeMusic;
using MusicTransify.src.Services.YouTubeMusic;
using MusicTransify.src.Controllers.Auth.Common.Login;

namespace MusicTransify.src.Controllers.Auth.YouTubeMusic
{
    [ApiController]
    [Route("auth/youtube")] // route "auth/youtube"
    public class YouTubeMusicAuthController : LoginController
    {
        private readonly YouTubeMusicAuthService _youTubeMusicAuthService;
        private readonly ILogger<YouTubeMusicAuthController> _logger;
        public YouTubeMusicAuthController(
            YouTubeMusicAuthService youTubeMusicAuthService,
            ILogger<LoginController> Baselogger,
            ILogger<YouTubeMusicAuthController> logger,
            Func<string, IPlatformAuthService> platformAuthFactory
        ) : base(Baselogger, platformAuthFactory)
        {
            _youTubeMusicAuthService = youTubeMusicAuthService;
            _logger = logger;
        }

        [HttpGet("login")] // Route: "/login"
        public IActionResult Login()
        {
            _logger.LogInformation("This is the Login route");

            // Set redirect URI
            string redirectUri = _youTubeMusicAuthService.GetLoginUri();

            return Redirect(redirectUri);
        }

        [HttpGet("callback")] // Route: "/callback"
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
            var tokenInfo = await _youTubeMusicAuthService.ExchangeAuthorizationCodeAsync(request.Code);

            // Redirect
            return Redirect("");
        }
    }
}