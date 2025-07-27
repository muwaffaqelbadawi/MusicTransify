using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.DataProtection;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Contracts.Services;
using MusicTransify.src.Infrastructure.Resilience.Spotify;
using MusicTransify.src.Infrastructure.Resilience.PolicyBuilder;
using MusicTransify.src.Middlewares;
using MusicTransify.src.Services.Auth.Spotify;
using MusicTransify.src.Services.Auth.YouTubeMusic;
using MusicTransify.src.Services.HTTP;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Services.Session;
using MusicTransify.src.Services.Cache;
using MusicTransify.src.Utilities.Token;
using MusicTransify.src.Utilities.Security;
using MusicTransify.src.Utilities.Auth.Common;
using MusicTransify.src.Utilities.Auth.Spotify;
using MusicTransify.src.Utilities.Auth.YouTubeMusic;
using MusicTransify.src.Contracts.Helper;

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

            // ========== SERVICE REGISTRATION ========== //
            builder.Services.AddHttpClient<IHttpService, HttpService>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(spotifyBaseUrl, UriKind.Absolute);
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue(applicationContentType));
                })
                .AddPolicyHandler(RetryPolicies.SpotifyRetryPolicy());

            // Configure Spotify client
            builder.Services.AddHttpClient("Spotify", client =>
            {
                client.BaseAddress = new Uri(spotifyBaseUrl, UriKind.Absolute);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(applicationContentType));
            });

            // Configure YouTube Music client
            builder.Services.AddHttpClient<YouTubeMusicService>("YouTube", client =>
            {
                client.BaseAddress = new Uri(youtubeBaseUrl, UriKind.Absolute);
            });

            // ========== REGISTER SERVICE WITH INTERFACE ========== //
            // Register Spotify service
            builder.Services.AddHttpClient<IProviderService, SpotifyService>(client =>
            {
                client.BaseAddress = new Uri(spotifyBaseUrl);
            })
            .AddPolicyHandler(PolicyBuilder.CreateResiliencePolicy());

            // Register YouTube Music service
            builder.Services.AddHttpClient<IProviderService, YouTubeMusicService>(client =>
            {
                client.BaseAddress = new Uri(youtubeBaseUrl, UriKind.Absolute);
            })
            .AddPolicyHandler(PolicyBuilder.CreateResiliencePolicy());

            // Register Spotify Helper
            builder.Services.AddHttpClient<IProviderHelper, SpotifyHelper>(client =>
            {
                client.BaseAddress = new Uri(spotifyBaseUrl);
            })
            .AddPolicyHandler(PolicyBuilder.CreateResiliencePolicy());

            // Register YouTube Music Helper
            builder.Services.AddHttpClient<IProviderHelper, YouTubeMusicHelper>(client =>
            {
                client.BaseAddress = new Uri(youtubeBaseUrl, UriKind.Absolute);
            })
            .AddPolicyHandler(PolicyBuilder.CreateResiliencePolicy());

            // Add Spotify content type
            builder.Services.AddControllers(options =>
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat(
                    applicationContentFormat, applicationContentType);
            });

            // Add YouTube Music content type
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

            // >NET built-in Cookies Service
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            });

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