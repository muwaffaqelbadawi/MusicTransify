using System;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Utilities.Common;

namespace MusicTransify.src.Controllers.Common
{
    [ApiController]
    [Route("")] // route "/"
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")] // route "/"
        public IActionResult Index()
        {
            _logger.LogInformation("Home page accessed");

            return this.PlainText("Welcome to the MusicTransify");
        }
    }
}
