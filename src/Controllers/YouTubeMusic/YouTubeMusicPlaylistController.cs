using System;
using System.Linq;
using System.Threading.Tasks;
using MusicTransify.src.Controllers.Common;


namespace MusicTransify.src.Controllers.YouTubeMusic
{
    public class YouTubeMusicPlaylistController : BaseApiController
    {
        public YouTubeMusicPlaylistController(ILogger<BaseApiController> logger) : base(logger)
        {
        }
    }
}