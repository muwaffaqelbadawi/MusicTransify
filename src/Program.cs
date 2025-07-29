using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.DataProtection;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Infrastructure.Resilience.Spotify;
using MusicTransify.src.Middlewares;
using MusicTransify.src.Services.Auth.Spotify;
using MusicTransify.src.Services.Auth.YouTubeMusic;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Services.Session;
using MusicTransify.src.Services.Cache;
using MusicTransify.src.Utilities.Token;
using MusicTransify.src.Utilities.Security;
using MusicTransify.src.Utilities.Auth.Common;
using MusicTransify.src.Utilities.Auth.Spotify;
using MusicTransify.src.Utilities.Auth.YouTubeMusic;
using MusicTransify.src.Infrastructure.Resilience.YouTubeMusic;
using MusicTransify.src.Services.HTTP.Spotify;
using MusicTransify.src.Contracts.Services.ProviderAuth.Spotify;
using MusicTransify.src.Contracts.Services.ProviderAuth.YouTubeMusic;
using MusicTransify.src.Contracts.Helper.Spotify;
using MusicTransify.src.Contracts.Helper.YouTubeMusic;
using MusicTransify.src.Contracts.Services.ProviderHttp.Spotify;
using MusicTransify.src.Contracts.Services.ProviderHttp.YouTubeMusic;
using MusicTransify.src.Services.HTTP.YouTubeMusic;

namespace MusicTransify.src
{
    class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ========== Configuration Urls ========== //
            // Frontend URL
            var frontendUrl = builder.Configuration.GetSection("Application:Frontend").Value ?? "";

            // Application name
            var appName = builder.Configuration.GetValue<string>("Application:Name") ?? "MusicTransify";

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy.WithOrigins(frontendUrl)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // ========== Spotify configurations ========== //
            var spotifyOptions = builder.Configuration.GetSection("Spotify");

            var spotifyBaseUrl = builder.Configuration.GetSection("Spotify:ApiBaseUri").Value
            ?? throw new InvalidOperationException("spotifyBaseUrl is missing");

            var applicationContentType = builder.Configuration.GetSection("Headers:ContentType").Value
            ?? throw new InvalidOperationException("spotify ContentType is missing");

            var applicationContentFormat = builder.Configuration.GetSection("Headers:ContentFormat").Value
            ?? throw new InvalidOperationException("spotify Spotify content format is missing");

            // ========== YouTube Music configurations ========== //
            var YouTubeMusicOptions = builder.Configuration.GetSection("YouTube");

            var youtubeBaseUrl = builder.Configuration.GetSection("YouTube:ApiBaseUri").Value
            ?? throw new InvalidOperationException("youtubeBaseUrl is missing");

            var userProfile = builder.Configuration.GetSection("Session:userProfile").Value
            ?? throw new InvalidOperationException("userProfile is missing");


            // ========== REGISTER SERVICE WITH INTERFACE ========== //

            // Register provider service (Spotify service)
            builder.Services.AddHttpClient<ISpotifyService, SpotifyService>(client =>
                {
                    client.BaseAddress = new Uri(spotifyBaseUrl);
                })
                .AddPolicyHandler(SpotifyRetryPolicy.Default());

            // Register provider service (YouTube Music service)
            builder.Services.AddHttpClient<IYouTubeMusicService, YouTubeMusicService>(client =>
                {
                    client.BaseAddress = new Uri(youtubeBaseUrl, UriKind.Absolute);
                })
                .AddPolicyHandler(YouTubeRetryPolicy.Default());

            // Register provider Helper (Spotify Helper)
            builder.Services.AddHttpClient<ISpotifyHelper, SpotifyHelper>(client =>
                {
                    client.BaseAddress = new Uri(spotifyBaseUrl);
                })
                .AddPolicyHandler(SpotifyRetryPolicy.Default());

            // Register provider Helper (YouTube Music Helper)
            builder.Services.AddHttpClient<IYouTubeMusicHelper, YouTubeMusicHelper>(client =>
                {
                    client.BaseAddress = new Uri(youtubeBaseUrl, UriKind.Absolute);
                })
                .AddPolicyHandler(YouTubeRetryPolicy.Default());

            // Register provider Http service (Spotify Http service)
            builder.Services.AddHttpClient<ISpotifyHttpService, SpotifyHttpService>("Spotify", client =>
                {
                    client.BaseAddress = new Uri(spotifyBaseUrl, UriKind.Absolute);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationContentType));
                })
                .AddPolicyHandler(SpotifyRetryPolicy.Default());

            // Registering the consumer
            builder.Services.AddTransient<SpotifyService>();


            // Register provider Http service (Spotify Http service)
            builder.Services.AddHttpClient<IYouTubeMusicHttpService, YouTubeMusicHttpService>("YouTube", client =>
                {
                    client.BaseAddress = new Uri(youtubeBaseUrl, UriKind.Absolute);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationContentType));
                })
                .AddPolicyHandler(YouTubeRetryPolicy.Default());

            // Registering the consumer
            builder.Services.AddTransient<YouTubeMusicService>();



            // >NET built-in Cookies Service
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            });

            // Add Spotify content type
            builder.Services.AddControllers(options =>
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat(
                    applicationContentFormat, applicationContentType);
            });

            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

            // ===== register Options =====
            // Register Spotify options
            builder.Services
            .AddOptions<SpotifyOptions>()
            .Bind(spotifyOptions)
            .ValidateDataAnnotations()
            .ValidateOnStart();

            // Register YouTube Music options
            builder.Services
            .AddOptions<YouTubeMusicOptions>()
            .Bind(YouTubeMusicOptions)
            .ValidateDataAnnotations()
            .ValidateOnStart();

            // ===== Register other services =====
            // Scoped
            builder.Services.AddTransient<SpotifyService>();
            builder.Services.AddTransient<YouTubeMusicService>();
            builder.Services.AddScoped<SessionService>();
            builder.Services.AddScoped<TokenHelper>();
            builder.Services.AddScoped<AuthHelper>();
            builder.Services.AddScoped<StateHelper>();
            builder.Services.AddScoped<SpotifyHelper>();
            builder.Services.AddScoped<YouTubeMusicHelper>();

            // Transient - Custom Cookies Service
            builder.Services.AddTransient<CookiesService>();

            // Infrastructure
            builder.Services.AddHttpContextAccessor();

            // Data Protection
            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(userProfile))
                .SetApplicationName(appName);
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

            // Use Routing
            app.UseRouting();

            // Use Cors
            app.UseCors("FrontendPolicy");

            // Use Sessions
            app.UseSession();

            // Endpoints
            app.MapControllers();

            // ========== Logging ========== //
            app.Logger.LogInformation("Application starting...");
            app.Logger.LogInformation("Application Name: {}", appName);
            app.Logger.LogInformation("frontendUrl: {}", frontendUrl);
            app.Logger.LogInformation("SpotifyBaseUrl: {}", spotifyBaseUrl);
            app.Logger.LogInformation("SpotifyContentType: {}", applicationContentType);
            app.Logger.LogInformation("applicationContentFormat: {}", applicationContentFormat);
            app.Logger.LogInformation("youtubeBaseUrl: {}", youtubeBaseUrl);
            app.Logger.LogInformation("userProfile: {}", userProfile);

            // ========== START APP ========== //
            app.Run();
        }
    }
}