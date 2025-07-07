using System;
using MusicTransify.src.Controllers.Common;
using MusicTransify.src.Services.Common;

namespace MusicTransify.src.Controllers.YouTubeMusic
{
    public class YouTubeMusicPlaylistController : AuthController
    {
        public YouTubeMusicPlaylistController(
            AuthService authService,
            SessionService sessionService,
            ILogger<AuthController> logger)
            : base(authService, sessionService, logger)
        {
        }
    }
}