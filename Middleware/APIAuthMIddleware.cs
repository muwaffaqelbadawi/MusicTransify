using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.Middleware
{
    public class APIAuthMIddleware
    {
        public class AuthenticationMiddleware(RequestDelegate next)
        {
            private readonly RequestDelegate _next = next;

            public async Task InvokeAsync(HttpContext context)
            {
                if (context.Request.Path.StartsWithSegments("/playlists") || context.Request.Path.StartsWithSegments("/refresh_token"))
                {
                    // if (string.IsNullOrEmpty(context.Session.GetString("access_token")) || Program.IsTokenExpired(context))
                    // {
                    //     context.Response.Redirect("/login");
                    //     return;
                    // }
                }

                // Call the next middleware in the pipeline
                await _next(context);
            }
        }
    }
}