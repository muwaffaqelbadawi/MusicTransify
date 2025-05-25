using System;
using System.Linq;
using System.Threading.Tasks;
using SpotifyWebAPI_Intro.src.Controllers.Common;


namespace SpotifyWebAPI_Intro.src.Controllers.YouTubeMusic
{
    public class YouTubeMusicPlaylistController : BaseApiController
    {
        public YouTubeMusicPlaylistController(ILogger<BaseApiController> logger) : base(logger)
        {
        }
    }
}