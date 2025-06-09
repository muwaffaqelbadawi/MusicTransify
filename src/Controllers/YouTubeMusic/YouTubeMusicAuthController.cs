using System;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Controllers.Common;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Utilities.Common;


namespace MusicTransify.src.Controllers.YouTubeMusic
{
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
    }
}