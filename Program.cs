using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpotifyWebAPI_Intro.Configuration;
using SpotifyWebAPI_Intro.Controllers;
using SpotifyWebAPI_Intro.Routes;
using SpotifyWebAPI_Intro.Services;

namespace SpotifyWebAPI_Intro
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureOptions<ApplicationOptions>();
            builder.Services.AddSingleton<ApplicationOptionsSetup>();
            builder.Services.AddHttpClient<HttpService>();

            var app = builder.Build();

            // Congigure the HTTP request pipline.
            if (!app.Environment.IsDevelopment())
            {
                // use session middleware
                app.UseSession();

                // Map routes
                app.MapGet("/", HomeController.Index);

                app.MapGet("/login", AuthController.Login);
                app.MapGet("/callback", AuthController.Callback);
                app.MapGet("/playlists", PlaylistsController.GetPlaylists);
                app.MapGet("/refresh_token", AuthController.RefreshToken);

                // Start app
                await app.RunAsync("http://localhost:5543");
            }
        }
    }
}