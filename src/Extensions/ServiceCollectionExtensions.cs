using System;
using Microsoft.AspNetCore.CookiePolicy;

namespace MusicTransify.src.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomWebDefaults(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // You should check if the frontend URL is valid
            var frontendUrl = configuration.GetSection("ApplicationUrls:Frontend").Value ?? "http://localhost:3000";

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontendDev", policy =>
                {
                    policy
                        .WithOrigins(frontendUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            // Cookies
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This is essential for the frontend
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = HttpOnlyPolicy.Always;
            });

            // Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".MusicTransify.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            return services;
        }
    }
}
