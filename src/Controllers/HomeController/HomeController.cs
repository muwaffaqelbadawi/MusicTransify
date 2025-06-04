using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Controllers.Common;

namespace MusicTransify.src.Controllers.HomeController
{
    [ApiController]
    [Route("/")] // Base route "/"
    public class HomeController : BaseApiController
    {
        private readonly new ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) : base(logger)
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