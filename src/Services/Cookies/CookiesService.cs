using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.src.Services.Cookies
{
    public class CookiesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CookiesService> _logger;
        public CookiesService(IHttpContextAccessor httpContextAccessor, ILogger<CookiesService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public void AppendCookies(string state)
        {
            if (_httpContextAccessor.HttpContext is not null)
            {
                if (string.IsNullOrEmpty(state))
                {
                    _logger.LogWarning("Attempted to append empty state cookie.");
                    return;
                }

                _httpContextAccessor.HttpContext.Response.Cookies.Append(
                "spotify_auth_state",
                state,
                new CookieOptions { HttpOnly = true, Secure = true }
                );
            }
        }
    }
}