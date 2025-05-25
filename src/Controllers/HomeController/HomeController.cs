using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SpotifyWebAPI_Intro.src.Controllers.HomeController
{
    [ApiController]
    [Route("/")] // Base route "/"
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet] // Route: "/"
        public IActionResult Index()
        {
            // Use the log information
            _logger.LogInformation("This is the Home route");

            // Return a welcome message with a login link
            string htmlContent = "<html>" +
            "<body>" +
            "<h1>Welcome to Spotify App</h1>" +
            "<a href='/auth/login'>Login with Spotify</a>" +
            "</body>" +
            "</html>";

            return Content(htmlContent, "text/html");
        }
    }
}