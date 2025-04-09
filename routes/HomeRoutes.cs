using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.Routes
{
    public class HomeRoutes
    {
        public static async Task Index(HttpContext context)
        {
            // Set the content type of the webpage
            context.Response.ContentType = "text/html";

            // Welcome message
            await context.Response.WriteAsync("Welcome to Spotify App <a href='/login'>Login with Spotify</a>");
        }
    }
}