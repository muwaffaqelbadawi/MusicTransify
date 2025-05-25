using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SpotifyWebAPI_Intro.src.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            // Store the next middleware in the pipeline
            _nextMiddleware = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the request path
            Console.WriteLine($"Request Path: {context.Request.Path}");

            // Call the next middleware in the pipeline
            await _nextMiddleware(context);
        }
    }
}