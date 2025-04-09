using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpotifyWebAPI_Intro.Configuration;
using SpotifyWebAPI_Intro.Routes;
using SpotifyWebAPI_Intro.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;





namespace SpotifyWebAPI_Intro
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services.ConfigureOptions<ApplicationOptions>();
      builder.Services.AddSingleton<ApplicationOptionsSetup>();

      var app = builder.Build();

      // Congigure the HTTP request pipline.
      if (!app.Environment.IsDevelopment())
      {

        // use session middleware
        app.UseSession();

        // use custom middleware without extension method
        // app.UseMiddleware<CustomMiddleware>();

        // use custom middleware with extension method
        //app.UseCustomMiddleware();

        // Map routes
        app.MapGet("/", HomeRoutes.Index);

        app.MapGet("/login", AuthRoutes.Login);
        app.MapGet("/callback", AuthRoutes.Callback);
        app.MapGet("/playlists", PlaylistsRoutes.GetPlaylists);
        app.MapGet("/refresh_token", AuthRoutes.RefreshToken);

        // Start app
        await app.RunAsync("http://localhost:5543");
      }
    }
  }
}