using System;
using Microsoft.AspNetCore.Mvc;

namespace MusicTransify.src.Controllers.Home
{
    [ApiController]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            _logger.LogInformation("Home page accessed");

            return Content(@"
                <!DOCTYPE html>
                <html>
                <body>
                    <h1>Welcome to MusicTransify</h1>
                    <div>
                        <a href='api/spotify/login' style='padding: 10px; background: green; color: white;'>Login with Spotify</a>
                        <a href='api/youtube/login' style='padding: 10px; background: red; color: white;'>Login with YouTube</a>
                    </div>
                </body>
                </html>",
                "text/html"
            );
        }
    }
}