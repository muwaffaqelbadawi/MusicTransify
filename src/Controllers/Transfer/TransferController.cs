using System;
using Microsoft.AspNetCore.Mvc;

namespace MusicTransify.src.Controllers.Transfer
{
    public class TransferController : Controller
    {
        private readonly ILogger<TransferController> _logger;
        public TransferController(ILogger<TransferController> logger)
        {
            _logger = logger;
        }
    }
}