using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using MusicTransify.src.Contracts.DTOs.Response.Token.YouTubeMusic;
using MusicTransify.src.Contracts.Session.YouTubeMusic;
using MusicTransify.src.Utilities.Token;

namespace MusicTransify.src.Services.Session.YouTubeMusic
{
    public class YouTubeMusicSessionService : IYouTubeMusicSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenHelper _tokenHelper;
        private readonly IDataProtector _protector;
        private readonly ILogger<YouTubeMusicSessionService> _logger;

        public YouTubeMusicSessionService(
            IHttpContextAccessor httpContextAccessor,
            TokenHelper token,
            IDataProtectionProvider protectionProvider,
            ILogger<YouTubeMusicSessionService> logger
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenHelper = token;
            _protector = protectionProvider.CreateProtector("TokenProtection");
            _logger = logger;
        }

        #region State Management
        public void StoreState(string state)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            httpContext?.Response.Cookies.Append("SpotifyState", state, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMinutes(5)
            });
        }

        public bool ValidateState(string state)
        {
            var httpContext = GetValidHttpContext();
            var storedState = httpContext?.Request.Cookies["SpotifyState"];
            return storedState == state;
        }
        #endregion

        #region Token Management
        public (
            string accessToken,
            string tokenType,
            int expiresIn,
            string refreshToken,
            string scope
        )
        ExtractTokenData(YouTubeMusicTokenResponse tokenInfo)
        {
            if (tokenInfo is null)
            {
                throw new ArgumentNullException(nameof(tokenInfo), "Missing required 'access_token' in response");
            }

            string accessToken = tokenInfo.AccessToken;
            string tokenType = tokenInfo.TokenType ?? "Bearer";
            int expiresIn = tokenInfo.ExpiresIn;
            string refreshToken = tokenInfo.RefreshToken;
            string scope = tokenInfo.Scope;

            return (accessToken, tokenType, expiresIn, refreshToken, scope);
        }

        public void Store(YouTubeMusicTokenResponse tokenInfo)
        {
            // ExtractTokenData existence of token assets
            var (accessToken, tokenType, expiresIn, refreshToken, scope) = ExtractTokenData(tokenInfo);

            if (refreshToken is null)
            {
                _logger.LogWarning("No refresh token provided");
            }

            var session = GetValidSession();
            string newExpiresIn = _tokenHelper.CalculateExpirationDate(expiresIn);

            if (session is null)
            {
                _logger.LogError("Session is not available");
                throw new InvalidOperationException("Session is not available");
            }

            session.SetString("access_token", accessToken);
            session.SetString("token_type", tokenType);

            if (!string.IsNullOrEmpty(refreshToken))
            {
                session.SetString("refresh_token", _protector.Protect(refreshToken));
            }

            session.SetString("expires_in", newExpiresIn);
            session.SetString("scope", scope);
            _logger.LogInformation("Successfully stored token info in session");
        }

        public void RegenerateSession()
        {
            var session = GetValidSession();
            session.Clear();
            session.CommitAsync().Wait();
        }

        public bool IsSessionValid()
        {
            try
            {
                var session = GetValidSession();
                return !string.IsNullOrEmpty(GetAccessToken());
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Private Helpers
        public string? GetProtectedToken(string tokenKey)
        {
            var session = _httpContextAccessor.HttpContext?.Session
                ?? throw new InvalidOperationException("Session unavailable");

            var encryptedValue = session.GetString(tokenKey);
            return !string.IsNullOrEmpty(encryptedValue)
                ? _protector.Unprotect(encryptedValue)
                : null;
        }

        public string? GetAccessToken() => GetProtectedToken("access_token");
        public string? GetRefreshToken() => GetProtectedToken("refresh_token");
        public DateTime? GetExpiration()
        {
            var expiresAt = _httpContextAccessor.HttpContext?.Session.GetString("expires_at");
            return DateTime.TryParse(expiresAt, out var result)
            ? result
            : null;
        }

        public string? GetTokenInfo(string tokenInfo)
        {
            var session = GetValidHttpContext().Session;

            if (!session.IsAvailable)
            {
                _logger.LogError("Session is not available");
                throw new InvalidOperationException("Session is not available");
            }

            return tokenInfo switch
            {
                "access_token" => session.GetString("access_token"),

                "token_type" => session.GetString("token_type"),

                "refresh_token" => session.GetString("refresh_token"),

                "expires_in" => session.GetString("expires_in"),

                "scope" => session.GetString("scope"),

                _ => throw new ArgumentException($"Invalid tokenInfo: {tokenInfo}", nameof(tokenInfo))
            };
        }
        #endregion

        #region Private Helpers
        private HttpContext GetValidHttpContext()
        {
            return _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP Context is not available");
        }

        private ISession GetValidSession()
        {
            var session = GetValidHttpContext().Session;

            if (!session.IsAvailable)
            {
                _logger.LogError("Session is not available");
                throw new InvalidOperationException("Session is not available");
            }
            return session;
        }
        #endregion
    }
}