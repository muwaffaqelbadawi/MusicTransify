using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SpotifyWebAPI_Intro.Configuration;
using SpotifyWebAPI_Intro.Services;

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

            // Add the Application Options configuration to the DI container
            builder.Services.ConfigureOptions<ApplicationOptions>();

            // Add the ApplicationOptionsSetup singleton to the DI container
            builder.Services.AddSingleton<ApplicationOptionsSetup>();

            // Add HttpClient to the DI container
            builder.Services.AddHttpClient<HttpService>();

            // Add session support
            builder.Services.AddDistributedMemoryCache();

            var app = builder.Build();

            // Configure the HTTP Request Pipeline
            // Enable session
            app.UseSession();

            // Enable authorization middleware
            app.UseAuthorization();

            // Configure routing for controllers
            app.UseRouting();

            // Map routes to controllers
            app.MapControllers();

            // Start app
            await app.RunAsync("http://localhost:5543");
        }
    }
}