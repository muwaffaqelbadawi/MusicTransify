using System;
using System.Threading.RateLimiting;
using MusicTransify.src.Contracts;
using MusicTransify.src.Middlewares;
using MusicTransify.src.Services.Spotify;
using MusicTransify.src.Services.YouTubeMusic;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Services.Common.Cookies;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Configurations.Common;
using MusicTransify.src.Utilities.Common;
using MusicTransify.src.Utilities.Security;


namespace MusicTransify.src
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a new WebApplicationBuilder instance
            var builder = WebApplication.CreateBuilder(args);

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // app URLs
            var urls = builder.Configuration.GetValue<string>("Application:Urls");

            // Read base URIs from configuration
            string? SpotifyBaseUri = builder.Configuration.GetValue<string>("ApiClients:Spotify:BaseUri");

            string? YouTubeMusicBaseUri = builder.Configuration.GetValue<string>("ApiClients:YouTubeMusic:BaseUri");

            if (string.IsNullOrEmpty(SpotifyBaseUri))
                throw new InvalidOperationException("Spotify BaseUri configuration is missing.");

            if (string.IsNullOrEmpty(YouTubeMusicBaseUri))
                throw new InvalidOperationException("YouTube Music BaseUri configuration is missing.");

            // health check
            // Add health check
            builder.Services.AddHealthChecks();

            // Controllers
            // Add controllers to the DI container
            builder.Services.AddControllers();

            // Http Context Accessor
            // Add AddHttpContextAccessor to the DI container
            builder.Services.AddHttpContextAccessor();

            //  Data Protection
            // Add Data Protection (fix for session middleware issue)
            builder.Services.AddDataProtection();

            // Add Rate Limiter
            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("api", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.User.Identity?.Name,
                        factory: _ => new()
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1)
                        }));
            });

            // Scoped
            // Add the SessionService singleton to the DI container
            builder.Services.AddScoped<SessionService>();

            // Scoped
            // Add TokenHelper to the DI container
            builder.Services.AddScoped<TokenHelper>();

            // Scoped
            // Add AuthHelper to the DI container
            builder.Services.AddScoped<AuthHelper>();

            // Transient
            // Add PlatformAuthService interface
            builder.Services.AddTransient<SpotifyAuthService>();
            builder.Services.AddTransient<YouTubeMusicAuthService>();
            builder.Services.AddTransient<Func<string, IPlatformAuthService>>(serviceProvider => key =>
            {
                return key.ToLower() switch
                {
                    "spotify" => serviceProvider.GetRequiredService<SpotifyAuthService>(),
                    "youtube" => serviceProvider.GetRequiredService<YouTubeMusicAuthService>(),
                    _ => throw new ArgumentException("Unknown platform", nameof(key))
                };
            });

            // Transient
            // Add CookiesService to the DI container
            builder.Services.AddTransient<CookiesService>();

            // Register Spotify Options
            builder.Services.ConfigureOptions<OptionsSetup<SpotifyOptions>>();

            // Register YouTube Music Options
            builder.Services.ConfigureOptions<OptionsSetup<YouTubeMusicOptions>>();

            // Register Common Options
            builder.Services.Configure<SpotifyOptions>(builder.Configuration.GetSection("Spotify"));

            // Register YouTube Music Options
            builder.Services.Configure<YouTubeMusicOptions>(builder.Configuration.GetSection("YouTubeMusic"));

            // Add secure cookies
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            });

            // HttpClient
            // Add HttpClient to the DI container
            builder.Services.AddHttpClient<HttpService>();

            // Add Spotify HttpClient to the DI container
            builder.Services.AddHttpClient<HttpService>("SpotifyApiClientService",
            client => client.BaseAddress = new Uri(SpotifyBaseUri));

            // Add YouTube Music HttpClient to the DI container
            builder.Services.AddHttpClient<HttpService>("YouTubeMusicApiClientService",
            client => client.BaseAddress = new Uri(YouTubeMusicBaseUri));

            // Distributed Memory Cache
            // Add session support
            builder.Services.AddDistributedMemoryCache();

            // Add session service to DI container
            // Session
            builder.Services.AddSession();

            // Initialize the app
            var app = builder.Build();

            // Enable healthcheck
            app.MapHealthChecks("/health");

            // Enable Cookie policy
            app.UseCookiePolicy();

            // Enable session
            app.UseSession();

            // Add the custom request logging middleware
            app.UseMiddleware<RequestLoggingMiddleware>();

            // Use CORS
            app.UseCors();

            // Configure session for controllers
            app.UseSession();

            // Configure routing for controllers
            app.UseRouting();

            // discovers all your [Route] controllers
            app.MapControllers();

            // Add a simple endpoint for debugging purposes
            app.MapGet("/debug", () =>
            {
                var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();
                return string.Join("\n", endpointDataSource.Endpoints
                    .Select(e => e.DisplayName));
            });

            // Start the app with the configured URLs
            await app.RunAsync(urls);
        }
    }
}