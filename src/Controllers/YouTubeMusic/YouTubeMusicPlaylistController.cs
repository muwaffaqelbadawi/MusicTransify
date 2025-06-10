using System;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Controllers.Common;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Utilities.Common;


namespace MusicTransify.src.Controllers.YouTubeMusic
{
    public class YouTubeMusicPlaylistController : AuthController
    {
        public YouTubeMusicPlaylistController(
            spotifyOptions spotifyOptions,
            AuthService authService,
            SessionService sessionService,
            TokenHelper tokenHelper,
            ILogger<AuthController> logger)
            : base(spotifyOptions, authService, sessionService, tokenHelper, logger)
        {
        }
    }
}