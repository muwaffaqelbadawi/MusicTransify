using System;
using Polly;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.DataProtection;
using MusicTransify.src.Contracts;
using MusicTransify.src.Services.Auth.Spotify;
using MusicTransify.src.Services.Auth.YouTubeMusic;
using MusicTransify.src.Services.HTTP;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Services.Session;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Utilities.Token;
using MusicTransify.src.Utilities.Security;
using MusicTransify.src.Middlewares;

namespace MusicTransify.src
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ========== Configuration Urls ========== //
            // Fronten Urls
            var nodeJsUrls = builder.Configuration.GetSection("Application:NodejsUrls").Get<string[]>()
            ?? throw new InvalidOperationException("Nodejs URLs are missing");

            // Application Utls
            var Urls = builder.Configuration.GetSection("Application:DotNetUrls").Get<string[]>()
                ?? throw new InvalidOperationException("Nodejs Urls are missing");

            // Frontend URL
            var frontendUrl = nodeJsUrls.FirstOrDefault()
                ?? throw new InvalidOperationException("Frontend URL is empty");

            if (!Uri.TryCreate(frontendUrl, UriKind.Absolute, out Uri? frontendUri))
            {
                throw new InvalidOperationException($"Invalid URL: {frontendUrl}");
            }

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy.WithOrigins(frontendUri.ToString())
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Application URL
            var AppUrl = Urls.FirstOrDefault()
                ?? throw new InvalidOperationException("Application URL is empty");

            if (!Uri.TryCreate(AppUrl, UriKind.Absolute, out Uri? AppUri))
            {
                throw new InvalidOperationException($"Invalid URL: {AppUrl}");
            }

            // Spotify configurations
            var spotifyBaseUrl = builder.Configuration.GetSection("SpotifyOptions:ApiBaseUri").Value
            ?? throw new InvalidOperationException("spotifyBaseUrl is missing");

            // Headers
            var spotifyContentType = builder.Configuration.GetSection("Headers:ContentType").Value
            ?? throw new InvalidOperationException("spotify ContentType is missing");

            var spotifyContentFormat = builder.Configuration.GetSection("Headers:ContentFormat").Value
            ?? throw new InvalidOperationException("spotify Spotify content format is missing");

            // YouTube Music configurations
            var youtubeBaseUrl = builder.Configuration.GetSection("YouTubeOptions:ApiBaseUri").Value
            ?? throw new InvalidOperationException("youtubeBaseUrl is missing");

            // Playlist configurations
            // PlaylistUrl configurations
            var userProfile = builder.Configuration.GetSection("Session:userProfile").Value
            ?? throw new InvalidOperationException("userProfile is missing");

            // ========== SERVICE REGISTRATION ========== //

            // Core infrastructure services
            builder.Services.AddHttpClient();
            builder.Services.AddLogging();

            builder.Services.AddHttpClient<IHttpService, HttpService>(client =>
            {
                client.BaseAddress = new Uri(spotifyBaseUrl, UriKind.Absolute);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(spotifyContentType));
            })
            .AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)));

            builder.Services.AddHttpClient<YouTubeMusicAuthService>(client =>
            {
                client.BaseAddress = new Uri(youtubeBaseUrl, UriKind.Absolute);
            });

            // Configure named clients if needed
            builder.Services.AddHttpClient("Spotify", client =>
            {
                client.BaseAddress = new Uri(spotifyBaseUrl, UriKind.Absolute);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(spotifyContentType));
            });

            builder.Services.AddControllers(options =>
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat(
                    spotifyContentFormat, spotifyContentType);
            });

            // ===== Other registrations =====
            builder.Services.AddTransient<SpotifyAuthService>();
            builder.Services.AddTransient<YouTubeMusicAuthService>();
            builder.Services.AddScoped<SessionService>();
            builder.Services.AddScoped<TokenHelper>();
            builder.Services.AddScoped<AuthHelper>();
            builder.Services.AddTransient<CookiesService>();

            // Configuration bindings
            builder.Services.Configure<SpotifyOptions>(builder.Configuration.GetSection("SpotifyOptions"));
            builder.Services.Configure<YouTubeMusicOptions>(builder.Configuration.GetSection("YouTubeOptions"));
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            });

            // Infrastructure
            builder.Services.AddHttpContextAccessor();


            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(userProfile))
                .SetApplicationName("MusicTransify");
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // ========== APP BUILDING ========== //
            var app = builder.Build();

            // ========== MIDDLEWARE PIPELINE ========== //
            // Early middleware
            app.UseMiddleware<Logging>();
            app.UseCookiePolicy();

            // Core middleware
            app.UseRouting();
            app.UseCors();
            app.UseSession();

            // Endpoints
            app.MapControllers();

            // ========== Logging ========== //
            app.Logger.LogInformation("Application starting...");
            app.Logger.LogInformation("AppUrl: {}", AppUrl);
            app.Logger.LogInformation("frontendUrl: {}", frontendUrl);
            app.Logger.LogInformation("SpotifyBaseUrl: {}", spotifyBaseUrl);
            app.Logger.LogInformation("SpotifyContentType: {}", spotifyContentType);
            app.Logger.LogInformation("SpotifyContentFormat: {}", spotifyContentFormat);
            app.Logger.LogInformation("youtubeBaseUrl: {}", youtubeBaseUrl);
            app.Logger.LogInformation("userProfile: {}", userProfile);

            // ========== START APP ========== //
            await app.RunAsync(AppUrl);
        }
    }
}