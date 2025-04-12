using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SpotifyWebAPI_Intro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("Home")]
        public async Task<IActionResult> Index(HttpContext context)
        {
            // Set the content type of the webpage
            context.Response.ContentType = "text/html";

            // Welcome message
            await context.Response.WriteAsync("Welcome to Spotify App <a href='/login'>Login with Spotify</a>");

            // Welcome message
            return Ok("Welcome to Spotify App <a href='/login'>Login with Spotify</a>");
        }
    }

}