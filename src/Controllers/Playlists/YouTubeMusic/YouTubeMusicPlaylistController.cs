using System;
using Microsoft.AspNetCore.Mvc;

namespace MusicTransify.src.Controllers.Playlists.YouTubeMusic
{
    public class YouTubeMusicPlaylistController : Controller
    {
        private readonly ILogger<YouTubeMusicPlaylistController> _logger;
        public YouTubeMusicPlaylistController (ILogger<YouTubeMusicPlaylistController> logger)
        {
            _logger = logger;
        }
    }
}