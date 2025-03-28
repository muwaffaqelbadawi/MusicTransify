using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using DotNetEnv;

class Program
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

    // Add services to the container.
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
      options.IdleTimeout = TimeSpan.FromMinutes(30);
      options.Cookie.HttpOnly = true;
      options.Cookie.IsEssential = true;
    });

    var app = builder.Build();

    // The Home page enpoint
    app.MapGet("/", async (HttpContext context) =>
   {
     context.Response.ContentType = "text/html";
     await context.Response.WriteAsync("Welcome to Spotify App <a href='/login'>Login with Spotify</a>");
     return Task.CompletedTask;
   });

    // login with Spotify enpoint
    app.MapGet("/login", async (HttpContext context) =>
    {
      context.Response.ContentType = "text/html";
      const string SCOPE = "user-read-private user-read-email";
      const string RESPONSE_TYPE = "code";

      var queryParameters = new
      {
        client_id = CLIENT_ID,
        response_type = RESPONSE_TYPE,
        scope = SCOPE,
        redirect_uri = REDIRECT_URI,
        show_dialog = true
      };

      var queryString = string.Join("&",
      queryParameters.GetType().GetProperties().Select(prop => $"{prop.Name}={prop.GetValue(queryParameters)}"));

      var auth_url = $"{AUTH_URL}?{queryString}";

      await Task.Run(() =>
      {
        context.Response.Redirect(auth_url);
      });

      return Task.CompletedTask;
    });

    // Callback
    app.MapGet("/callback", async (HttpContext context) =>
    {
      context.Response.ContentType = "text/html";
      // Check for "error" in the query string
      if (context.Request.Query.ContainsKey("error"))
      {
        await context.Response.WriteAsJsonAsync(new { error = context.Request.Query["error"] });
        return Task.CompletedTask;
      }

      // Check for "error" in the query string
      if (context.Request.Query.ContainsKey("code"))
      {
        var req_body = new
        {
          code = context.Request.Query["code"],
          grant_type = "authorization_code",
          redirect_url = REDIRECT_URI,
          client_id = CLIENT_ID,
          client_secret = CLIENT_SECRET
        };

        var json = JsonSerializer.Serialize(req_body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        var response = await client.PostAsync(TOKEN_URL, content);

        // Getting the result as a response
        var result = await response.Content.ReadAsStringAsync();

        // Deserialize token_info into a dynamic object or a strongly-typed class
        var token_info = JsonSerializer.Deserialize<JsonElement>(result);

        string access_token = token_info.GetProperty("access_token").ToString();
        string refresh_token = token_info.GetProperty("refresh_token").ToString();
        double expires_in = token_info.GetProperty("expires_in").GetDouble();
        string expires_at = DateTimeOffset.UtcNow.AddSeconds(expires_in).ToUnixTimeSeconds().ToString();

        // Store token information in session
        context.Session.SetString("access_token", access_token);
        context.Session.SetString("refresh_token", refresh_token);
        context.Session.SetString("expires_in", expires_at);

        context.Response.Redirect("/playlists");
        return Task.CompletedTask;
      }

      // Handle the case where neither "error" nor "code" is present
      await context.Response.WriteAsJsonAsync(new { error = "Invalid request" });
      return Task.CompletedTask;
    });

    // Playlists endpoint
    app.MapGet("/playlists", async (HttpContext context) =>
    {
      context.Response.ContentType = "text/html";
      if (!context.Request.Query.ContainsKey("access_token"))
      {
        context.Response.Redirect("/login");
        return Task.CompletedTask;
      }

      long expires_at = long.Parse(context.Session.GetString("expires_at") ?? throw new InvalidOperationException("No expires_at found"));

      long current_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

      if (current_time > expires_at)
      {
        Console.WriteLine("TOKEN EXPIRED. REFRESHING...");
        context.Response.Redirect("/refresh_token");
        return Task.CompletedTask;
      }

      string Authorization = $"Bearer {context.Session.GetString("access_token")}";

      using var client = new HttpClient();
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Authorization);

      var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
      var response = await client.PostAsync($"{API_BASE_URL}me/playlists", content);
      var result = await response.Content.ReadAsStringAsync();

      var playlists = JsonSerializer.Deserialize<JsonElement>(result);
      await context.Response.WriteAsJsonAsync(playlists);
      return Task.CompletedTask;
    });

    app.MapGet("/refresh_token", async (HttpContext context) =>
    {
      return Task.CompletedTask;
    });

    await app.RunAsync("http://localhost:5543");
  }
}