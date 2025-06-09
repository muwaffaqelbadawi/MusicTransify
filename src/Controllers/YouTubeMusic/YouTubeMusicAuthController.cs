using System;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Controllers.Common;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Utilities.Common;


namespace MusicTransify.src.Controllers.YouTubeMusic
{
    [ApiController]
    [Route("auth/music.youtube")] // route "/auth/music.youtube"
    public class YouTubeMusicAuthController : AuthController
    {
        public YouTubeMusicAuthController(
            SpotifyOptionsProvider spotifyOptionsProvider,
            AuthService authService,
            SessionService sessionService,
            TokenHelper tokenHelper,
            ILogger<AuthController> logger)
            : base(spotifyOptionsProvider, authService, sessionService, tokenHelper, logger)
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



    }
}