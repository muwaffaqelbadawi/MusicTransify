using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SpotifyWebAPI_Intro.Controllers
{
    [ApiController]
    [Route("/")] // Handle the root URL
    public class HomeController : ControllerBase
    {
        [HttpGet] // Handle GET requests to "/"
        public IActionResult Index()
        {
            // Return a welcome message with a login link
            string htmlContent = "<html>" +
            "<body>" +
            "<h1>Welcome to Spotify App</h1>" +
            "<a href='/login'>Login with Spotify</a>" +
            "</body>" +
            "</html>";

            return Content(htmlContent, "text/html");
        }
    }

}