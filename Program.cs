using System;
using Microsoft.Extensions.Options;
using SpotifyWebAPI_Intro.Configuration;
using SpotifyWebAPI_Intro.Services;
using SpotifyWebAPI_Intro.Utilities;
using SpotifyWebAPI_Intro.Middlewares;

namespace SpotifyWebAPI_Intro
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a new WebApplicationBuilder instance
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            // Add controllers to the DI container
            builder.Services.AddControllers();

            // Http Context Accessor
            // Add AddHttpContextAccessor to the DI container
            builder.Services.AddHttpContextAccessor();

            //  Data Protection
            // Add Data Protection (fix for session middleware issue)
            builder.Services.AddDataProtection();

            // Scoped
            // Add the SessionService singleton to the DI container
            builder.Services.AddScoped<SessionService>();

            // Scoped
            // Add TokenHelper to the DI container
            builder.Services.AddScoped<TokenHelper>();

            // Scoped
            // Add the AuthService singleton to the DI container
            builder.Services.AddScoped<AuthService>();

            // Scoped
            // Add AuthHelper to the DI container
            builder.Services.AddScoped<AuthHelper>();

            //  Singleton
            // Add the ApplicationOptionsSetup singleton to the DI container
            builder.Services.AddSingleton<IConfigureOptions<ApplicationOptions>,
            ApplicationOptionsSetup>();

            // Singleton
            // Add the OptionsService singleton to the DI container
            builder.Services.AddSingleton<OptionsService>();

            //  HttpClient
            // Add HttpClient to the DI container
            builder.Services.AddHttpClient<HttpService>();

            // Distributed Memory Cache
            // Add session support
            builder.Services.AddDistributedMemoryCache();

            // Session
            // Add session service to DI container
            builder.Services.AddSession();

            // Initialize the app
            var app = builder.Build();

            // Enable Cookie policy
            app.UseCookiePolicy();

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

            // Start the app
            await app.RunAsync("http://localhost:5543");
        }
    }
}