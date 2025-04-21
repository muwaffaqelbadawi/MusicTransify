using System;
using Microsoft.Extensions.Options;
using SpotifyWebAPI_Intro.Configuration;
using SpotifyWebAPI_Intro.Services;
using SpotifyWebAPI_Intro.utilities;
using SpotifyWebAPI_Intro.Middlewares;

namespace SpotifyWebAPI_Intro
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a new WebApplicationBuilder instance
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container

            // Add the Required Services
            // Add controllers to the DI container
            builder.Services.AddControllers();

            // Add AddHttpContextAccessor to the DI container
            builder.Services.AddHttpContextAccessor();

            // Add the ApplicationOptionsSetup singleton to the DI container
            builder.Services.AddSingleton<IConfigureOptions<ApplicationOptions>,
            ApplicationOptionsSetup>();

            // Add Data Protection (fix for session middleware issue)
            builder.Services.AddDataProtection();

            // Add the OptionsService singleton to the DI container
            builder.Services.AddSingleton<OptionsService>();

            // Add the SessionService singleton to the DI container
            builder.Services.AddSingleton<SessionService>();

            // Add the AuthService singleton to the DI container
            builder.Services.AddSingleton<AuthService>();

            // Add HttpClient to the DI container
            builder.Services.AddHttpClient<HttpService>();

            // Add AuthHelper to the DI container
            builder.Services.AddSingleton<AuthHelper>();

            // Add session support
            builder.Services.AddDistributedMemoryCache();

            // Add session service to DI container
            builder.Services.AddSession();

            // Enforcing url lower case
            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            var app = builder.Build();

            // Enable session
            app.UseSession();

            // Serve default files
            // app.UseDefaultFiles();

            // Enable static files middleware
            // app.UseStaticFiles();

            // Configure routing for controllers
            app.UseRouting();

            // Add the custom request logging middleware
            app.UseMiddleware<RequestLoggingMiddleware>();

            // Map routes to controllers
            app.MapControllers();

            // Start app
            await app.RunAsync("http://localhost:5543");
        }
    }
}