using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.DataProtection;
using MusicTransify.src.Contracts.HTTP;
using MusicTransify.src.Contracts.Spotify;
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
using MusicTransify.src.Infrastructure.Resilience.PolicyBuilder;
using MusicTransify.src.Contracts.YouTubeMusic;
using MusicTransify.src.Infrastructure.Resilience.Spotify;
using MusicTransify.src.Services.Cache;

namespace MusicTransify.src
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ========== Configuration Urls ========== //
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
            var spotifyOptions = builder.Configuration.GetSection("Spotify");

            var spotifyBaseUrl = builder.Configuration.GetSection("Spotify:ApiBaseUri").Value
            ?? throw new InvalidOperationException("spotifyBaseUrl is missing");

            var applicationContentType = builder.Configuration.GetSection("Headers:ContentType").Value
            ?? throw new InvalidOperationException("spotify ContentType is missing");

            var applicationContentFormat = builder.Configuration.GetSection("Headers:ContentFormat").Value
            ?? throw new InvalidOperationException("spotify Spotify content format is missing");

            // YouTube Music configurations
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

            // Register Spotify service
            builder.Services.AddHttpClient<ISpotifyService, SpotifyService>(client =>
            {
                client.BaseAddress = new Uri(spotifyBaseUrl);
            })
            .AddPolicyHandler(PolicyBuilder.CreateResiliencePolicy());

            // Register YouTube Music service
            builder.Services.AddHttpClient<IYouTubeMusicService, IYouTubeMusicService>(client =>
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


            // ===== Other registrations =====
            builder.Services.AddTransient<SpotifyService>();
            builder.Services.AddTransient<YouTubeMusicService>();
            builder.Services.AddScoped<SessionService>();
            builder.Services.AddScoped<TokenHelper>();
            builder.Services.AddScoped<AuthHelper>();
            builder.Services.AddTransient<CookiesService>();

            // Configuration bindings
            builder.Services.Configure<SpotifyOptions>(spotifyOptions);
            builder.Services.Configure<YouTubeMusicOptions>(builder.Configuration.GetSection("YouTube"));
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
            app.Logger.LogInformation("SpotifyContentType: {}", applicationContentType);
            app.Logger.LogInformation("applicationContentFormat: {}", applicationContentFormat);
            app.Logger.LogInformation("youtubeBaseUrl: {}", youtubeBaseUrl);
            app.Logger.LogInformation("userProfile: {}", userProfile);

            // ========== START APP ========== //
            await app.RunAsync(AppUrl);
        }
    }
}