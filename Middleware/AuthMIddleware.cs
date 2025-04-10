using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.Middleware
{
    public class AuthMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}