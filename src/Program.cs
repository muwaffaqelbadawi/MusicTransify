using System;
using MusicTransify.src.Middlewares;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Configurations.Common;
using MusicTransify.src.Utilities.Common;
using MusicTransify.src.Services.Transfer;
using MusicTransify.src.Utilities.Security;
using MusicTransify.src.Contracts;

namespace MusicTransify.src
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a new WebApplicationBuilder instance
            var builder = WebApplication.CreateBuilder(args);

            // app URLs
            var urls = builder.Configuration.GetValue<string>("Application:Urls");

            // client baseUri
            string baseUri = "";

            // health check
            // Add health check
            builder.Services.AddHealthChecks();

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

            builder.Services.AddScoped<IMatchingService, FuzzyMatchingService>();

            // Scoped
            // Add TokenHelper to the DI container
            builder.Services.AddScoped<TokenHelper>();

            // Scoped
            // Add the AuthService singleton to the DI container
            builder.Services.AddScoped<AuthService>();

            // Scoped
            // Add AuthHelper to the DI container
            builder.Services.AddScoped<AuthHelper>();

            // Register Spotify Options
            builder.Services.ConfigureOptions<OptionsSetup<SpotifyOptions>>();

            // Register YouTube Music Options
            builder.Services.ConfigureOptions<OptionsSetup<YouTubeMusicOptions>>();

            //  HttpClient
            // Add HttpClient to the DI container
            builder.Services.AddHttpClient<HttpService>();

            // Add Spotify HttpClient to the DI container
            builder.Services.AddHttpClient<HttpService>("SpotifyApiClientService",
            client => client.BaseAddress = new Uri(baseUri));

            // Add YouTube Music HttpClient to the DI container
            builder.Services.AddHttpClient<HttpService>("YouTubeMusicApiClientService",
            client => client.BaseAddress = new Uri(baseUri));

            // Distributed Memory Cache
            // Add session support
            builder.Services.AddDistributedMemoryCache();

            // Session
            // Add session service to DI container
            builder.Services.AddSession();

            // Initialize the app
            var app = builder.Build();

            app.MapHealthChecks("/health");

            // Enable Cookie policy
            app.UseCookiePolicy();

            // Enable session
            app.UseSession();

            // Configure routing for controllers
            app.UseRouting();

            // Add the custom request logging middleware
            app.UseMiddleware<RequestLoggingMiddleware>();

            // Map routes to controllers
            app.MapControllers();

            // Start the app with the configured URLs
            await app.RunAsync(urls);
        }
    }
}