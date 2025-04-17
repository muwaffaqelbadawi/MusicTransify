using System;
using Microsoft.Extensions.Options;
using SpotifyWebAPI_Intro.Configuration;
using SpotifyWebAPI_Intro.Services;
using SpotifyWebAPI_Intro.utilities;

namespace SpotifyWebAPI_Intro
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a new WebApplicationBuilder instance
            var builder = WebApplication.CreateBuilder(args);

            // Add the Required Services
            // Add controllers to the DI container
            builder.Services.AddControllers();

            // Add AddHttpContextAccessor to the DI container
            builder.Services.AddHttpContextAccessor();

            // Add the ApplicationOptionsSetup singleton to the DI container
            builder.Services.AddSingleton<IConfigureOptions<ApplicationOptions>, ApplicationOptionsSetup>();

            // Add Data Protection (fix for session middleware issue)
            builder.Services.AddDataProtection();

            // Add the SessionService singleton to the DI container
            builder.Services.AddSingleton<SessionService>();

            // Add HttpClient to the DI container
            builder.Services.AddHttpClient<HttpService>();

            // Add AuthHelper to the DI container
            builder.Services.AddSingleton<AuthHelper>();

            // Add session support
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP Request Pipeline
            // Enable session
            app.UseSession();

            // Serve default files
            app.UseDefaultFiles();

            // Enable static files middleware
            app.UseStaticFiles();

            // Configure routing for controllers
            app.UseRouting();

            // Map routes to controllers
            app.MapControllers();

            // Fallback to index.html for client-side routing
            app.MapFallbackToFile("index.html");

            // Start app
            await app.RunAsync("http://localhost:5543");
        }
    }
}