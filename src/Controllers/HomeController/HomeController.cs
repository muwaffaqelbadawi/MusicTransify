using System;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Controllers.Common;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Utilities.Common;

namespace MusicTransify.src.Controllers.HomeController
{
    [ApiController]
    [Route("/")] // Base route "/"
    public class HomeController : AuthController
    {
        private readonly new ILogger<HomeController> _logger;

        public HomeController(
            SpotifyOptionsProvider spotifyOptionsProvider,
            AuthService authService,
            SessionService sessionService,
            TokenHelper tokenHelper,
            ILogger<AuthController> baseLogger,
            ILogger<HomeController> logger
        ) : base(spotifyOptionsProvider, authService, sessionService, tokenHelper, baseLogger)
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