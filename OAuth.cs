using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;

class OAuthApp
{
  // Fetch environment variables and provide default values or throw exceptions if not set
  private static string CLIENT_ID => Environment.GetEnvironmentVariable("Spotify_CLIENT_ID")
  ?? throw new InvalidOperationException("Spotify_CLIENT_ID is not set in environment variables");
  private static string CLIENT_SECRET => Environment.GetEnvironmentVariable("Spotify_CLIENT_SECRET")
  ?? throw new InvalidOperationException("Spotify_CLIENT_SECRET is not set in environment variables");
  private static string REDIRECT_URI => Environment.GetEnvironmentVariable("Spotify_REDIRECT_URI")
  ?? throw new InvalidOperationException("Spotify_REDIRECT_URI is not set in environment variables");
  private static string AUTH_URL => Environment.GetEnvironmentVariable("Spotify_AUTH_URL")
  ?? throw new InvalidOperationException("Spotify_AUTH_URL is not set in environment variables");
  private static string TOKEN_URL => Environment.GetEnvironmentVariable("Spotify_TOKEN_URL")
  ?? throw new InvalidOperationException("Spotify_TOKEN_URL is not set in environment variables");
  private static string API_BASE_URL => Environment.GetEnvironmentVariable("Spotify_API_BASE_URL")
  ?? throw new InvalidOperationException("Spotify_API_BASE_URL is not set in environment variables");
  static async Task Main(string[] args)
  {
    // Make sure "http://localhost:5543/callback" is in your application's redirect URIs!
    // Load environment variables from project root .env file
    Env.Load();

    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();


    // The first Route Endpoint
    app.MapGet("/", (HttpContext context) =>
   {
     context.Response.ContentType = "text/html";
     return context.Response.WriteAsync("Welcome to Spotify App <a href='/login'>Login with Spotify</a>");
     // return Task.CompletedTask;
   });

    app.MapGet("/login", (HttpContext context) =>
    {
      const string SCOPE = "user-read-private user-read-email";
      const string RESPONSE_TYPE = "code";

      var queryParameters = new
      {
        client_id = CLIENT_ID,
        response_type = RESPONSE_TYPE,
        scope = SCOPE,
        redirect_uri = REDIRECT_URI,
      };

      var queryString = string.Join("&",
      queryParameters.GetType().GetProperties().Select(prop => $"{prop.Name}={prop.GetValue(queryParameters)}"));

      var auth_url = $"{AUTH_URL}?{queryString}";
      context.Response.Redirect(auth_url);
      return Task.CompletedTask;
    });
    await app.RunAsync("http://localhost:5543");
  }
}