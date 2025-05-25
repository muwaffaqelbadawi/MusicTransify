using System;
using System.Linq;
using System.Threading.Tasks;
using SpotifyWebAPI_Intro.src.Controllers.Common;


namespace SpotifyWebAPI_Intro.src.Controllers.Transfer
{
    public class TransferController : BaseApiController
    {
        public TransferController(ILogger<BaseApiController> logger) : base(logger)
        {
        }
    }
}