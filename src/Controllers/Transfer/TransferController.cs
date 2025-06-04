using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.Services.Spotify;
using MusicTransify.src.Controllers.Common;
using MusicTransify.src.Dtos;
using MusicTransify.src.Models.Spotify;


namespace MusicTransify.src.Controllers.Transfer
{
    // Auto mapper


    public class TransferController : BaseApiController
    {

        public TransferController(ILogger<BaseApiController> logger) : base(logger)
        {
        }




    }
}