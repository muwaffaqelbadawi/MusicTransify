// Global using
using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.DataProtection;

// Configurations
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Configurations.YouTubeMusic;

// Contracts
using MusicTransify.src.Contracts.Services.Auth.Spotify;
using MusicTransify.src.Contracts.Services.Auth.YouTubeMusic;
using MusicTransify.src.Contracts.Services.Http.Spotify;
using MusicTransify.src.Contracts.Services.Http.YouTubeMusic;
using MusicTransify.src.Contracts.Services.Playlists.YouTubeMusic;
using MusicTransify.src.Contracts.Utilities.Spotify;
using MusicTransify.src.Contracts.Utilities.YouTubeMusic;
using MusicTransify.src.Contracts.Session.Spotify;
using MusicTransify.src.Contracts.Session.YouTubeMusic;

// Extensions
using MusicTransify.src.Extensions;

// Infrastructure
using MusicTransify.src.Infrastructure.RetryPolicy.Spotify;
using MusicTransify.src.Infrastructure.RetryPolicy.YouTubeMusic;

// Middlewares
using MusicTransify.src.Middlewares;

// Services
using MusicTransify.src.Services.Auth.Spotify;
using MusicTransify.src.Services.Auth.YouTubeMusic;
using MusicTransify.src.Services.Cache;
using MusicTransify.src.Services.Cookies;
using MusicTransify.src.Services.Http.Spotify;
using MusicTransify.src.Services.Http.YouTubeMusic;
using MusicTransify.src.Services.Playlists.Spotify;
using MusicTransify.src.Services.Playlists.YouTubeMusic;
using MusicTransify.src.Services.Session.Spotify;
using MusicTransify.src.Services.Session.YouTubeMusic;

// Utilities
using MusicTransify.src.Utilities.Auth.Common;
using MusicTransify.src.Utilities.Auth.Spotify;
using MusicTransify.src.Utilities.Auth.YouTubeMusic;
using MusicTransify.src.Utilities.Options.Spotify;
using MusicTransify.src.Utilities.Options.YouTubeMusic;
using MusicTransify.src.Utilities.Security;
using MusicTransify.src.Utilities.Token;
using MusicTransify.src.Utilities.Playlists.Spotify;
using MusicTransify.src.Utilities.Playlists.YouTubeMusic;
using MusicTransify.src.Utilities.Session.Spotify;
using MusicTransify.src.Utilities.Session.YouTubeMusic;

namespace MusicTransify.src
{
    class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Access environment
            var env = builder.Environment;

            if (env.IsDevelopment())
            {
                // Extensions
                builder.Services.AddCustomWebDefaults(builder.Configuration);

                // ========== Spotify configurations ========== //
                var spotifyOptions = builder.Configuration.GetSection("Spotify");

                var spotifyBaseUrl = builder.Configuration.GetSection("Spotify:ApiBaseUri").Value
                ?? throw new InvalidOperationException("spotifyBaseUrl is missing");

                var applicationContentType = builder.Configuration.GetSection("Headers:ContentType").Value
                ?? throw new InvalidOperationException("spotify ContentType is missing");

                var applicationContentFormat = builder.Configuration.GetSection("Headers:ContentFormat").Value
                ?? throw new InvalidOperationException("spotify Spotify content format is missing");

                // ========== YouTube Music configurations ========== //
                var youtubeMusicOptions = builder.Configuration.GetSection("YouTube");

                var youtubeBaseUrl = builder.Configuration.GetSection("YouTube:ApiBaseUri").Value
                ?? throw new InvalidOperationException("youtubeBaseUrl is missing");

                var userProfile = builder.Configuration.GetSection("Session:userProfile").Value
                ?? throw new InvalidOperationException("userProfile is missing");

                // ========== REGISTER SERVICE WITH INTERFACE ========== //

                // Register provider Http service (Spotify Http service)
                builder.Services.AddHttpClient<ISpotifyHttpService, SpotifyHttpService>("Spotify", client =>
                {
                    client.BaseAddress = new Uri(spotifyBaseUrl, UriKind.Absolute);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationContentType));
                })
                .AddPolicyHandler(SpotifyRetryPolicy.Default());

                // Registering the Spotify consumer
                builder.Services.AddTransient<SpotifyService>();

                // Register provider Http service (YouTube Music Http service)
                builder.Services.AddHttpClient<IYouTubeMusicHttpService, YouTubeMusicHttpService>("YouTube", client =>
                {
                    client.BaseAddress = new Uri(youtubeBaseUrl, UriKind.Absolute);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationContentType));
                })
                .AddPolicyHandler(YouTubeMusicRetryPolicy.Default());

                // Registering the YouTube Music consumer
                builder.Services.AddTransient<YouTubeMusicService>();

                // Register provider service (Spotify service)
                // This service doesn't have HttpClient in the constructor
                builder.Services.AddScoped<ISpotifyService, SpotifyService>();

                // Register provider service (YouTube Music service)
                // This service doesn't have HttpClient in the constructor
                builder.Services.AddScoped<IYouTubeMusicService, YouTubeMusicService>();

                // Register playlist provider service (YouTube Music playlist service)
                builder.Services.AddTransient<IYouTubeMusicPlaylistsService, YouTubeMusicPlaylistsService>();

                // Register provider Helper (Spotify Helper)
                builder.Services.AddTransient<ISpotifyHelper, SpotifyHelper>();

                // Register provider Helper (YouTube Music Helper)
                builder.Services.AddTransient<IYouTubeMusicHelper, YouTubeMusicHelper>();

                // Register provider playlist Helper (Spotify Playlist Helper)
                builder.Services.AddTransient<ISpotifyPlaylistsHelper, SpotifyPlaylistsHelper>();

                // Register provider playlist Helper (YouTube Music Playlist Helper)
                builder.Services.AddTransient<IYouTubeMusicPlaylistsHelper, YouTubeMusicPlaylistsHelper>();

                // Register provider session (Spotify session)
                builder.Services.AddTransient<ISpotifySessionService, SpotifySessionService>();

                // Register provider session (YouTube Music session)
                builder.Services.AddTransient<IYouTubeMusicSessionService, YouTubeMusicSessionService>();

                // Add Spotify content type
                builder.Services.AddControllers(options =>
                    {
                        options.FormatterMappings.SetMediaTypeMappingForFormat(
                        applicationContentFormat, applicationContentType);
                    }
                );

                builder.Services.AddMemoryCache();

                // Singleton
                builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

                // Singleton
                builder.Services.AddSingleton<SpotifyOptionsHelper>();
                builder.Services.AddSingleton<YouTubeMusicOptionsHelper>();

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
                .Bind(youtubeMusicOptions)
                .ValidateDataAnnotations()
                .ValidateOnStart();

                // ===== Register other services =====
                // Transient - Custom Cookies Service
                builder.Services.AddTransient<CookiesService>();

                // Transient
                builder.Services.AddTransient<SpotifyService>();
                builder.Services.AddTransient<YouTubeMusicService>();
                builder.Services.AddTransient<SpotifyTokenInfoHelper>();
                builder.Services.AddTransient<YouTubeMusicTokenInfoHelper>();
                builder.Services.AddTransient<SpotifyPlaylistsQueryBuilder>();
                builder.Services.AddTransient<YouTubeMusicPlaylistsQueryBuilder>();

                // Scoped
                builder.Services.AddScoped<SpotifySessionService>();
                builder.Services.AddScoped<YouTubeMusicSessionService>();
                builder.Services.AddScoped<TokenHelper>();
                builder.Services.AddScoped<AuthHelper>();
                builder.Services.AddScoped<StateHelper>();
                builder.Services.AddScoped<SpotifyHelper>();
                builder.Services.AddScoped<YouTubeMusicHelper>();

                // Infrastructure
                builder.Services.AddHttpContextAccessor();

                // Data Protection
                builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(userProfile))
                .SetApplicationName("MusicTransify");

                
            }
            
            // ========== APP BUILDING ========== //
            var app = builder.Build();

            // ========== MIDDLEWARE PIPELINE ========== //
            if (env.IsDevelopment())
            {
                app.UseCors("AllowFrontendDev");
            }
            else
            {
                app.UseCors("AllowFrontendProd");
            }

            // Use Routing
            app.UseRouting();

            // Custom middleware
            app.UseMiddleware<Logging>();

            // Use CookiePolicy
            app.UseCookiePolicy();

            // Use Session
            app.UseSession();

            // Controller Endpoints
            app.MapControllers();

            // ========== START APP ========== //
            app.Run();
        }
    }
}