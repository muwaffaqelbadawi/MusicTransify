using System;
using System.Linq;
using System.Threading.Tasks;
using SpotifyWebAPI_Intro.src.Controllers.Common;


namespace SpotifyWebAPI_Intro.src.Controllers.YouTubeMusic
{
    public class YouTubeMusicAuthController : BaseApiController
    {
        public YouTubeMusicAuthController(ILogger<BaseApiController> logger) : base(logger)
        {
        }
    }
}