using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpotifyWebAPI_Intro.Services.Spotify;
using SpotifyWebAPI_Intro.src.Controllers.Common;
using SpotifyWebAPI_Intro.src.Dtos;
using SpotifyWebAPI_Intro.src.Models.Spotify;


namespace SpotifyWebAPI_Intro.src.Controllers.Transfer
{
    // Auto mapper

    
    public class TransferController : BaseApiController
    {

        public TransferController(ILogger<BaseApiController> logger) : base(logger)
        {
        }



        
    }
}