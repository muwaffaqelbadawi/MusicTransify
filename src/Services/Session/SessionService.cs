using System;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using MusicTransify.src.Utilities.Token;

namespace MusicTransify.src.Services.Session
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<SessionService> _logger;
        private readonly IDataProtector _protector;

        public SessionService(
            IHttpContextAccessor httpContextAccessor,
            TokenHelper token,
            ILogger<SessionService> logger,
            IDataProtectionProvider protectionProvider
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenHelper = token;
            _logger = logger;
            _protector = protectionProvider.CreateProtector("TokenProtection");
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
        public (string accessToken, string tokenType, int expiresIn, string? refreshToken, string scope)
        Check(JsonElement tokenInfo)
        {
            try
            {
                string accessToken = tokenInfo.GetProperty("access_token").GetString()
                    ?? throw new InvalidOperationException("No 'access_token' found");

                string tokenType = tokenInfo.GetProperty("token_type").GetString()
                    ?? "Bearer";

                int expiresIn = tokenInfo.GetProperty("expires_in").GetInt32();

                tokenInfo.TryGetProperty("refresh_token", out JsonElement refreshTokenElement);
                string? refreshToken = refreshTokenElement.ValueKind != JsonValueKind.Undefined
                    ? refreshTokenElement.GetString()
                    : null;

                string scope = tokenInfo.GetProperty("scope").GetString()
                    ?? "default_scope";

                return (accessToken, tokenType, expiresIn, refreshToken, scope);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                "Failed to extract tokens: {TokenInfo}",
                tokenInfo);

                throw;
            }
        }

        public void Store(JsonElement tokenInfo)
        {
            // Check existence of token assets
            var (accessToken, tokenType, expiresIn, refreshToken, scope) = Check(tokenInfo);

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
            return DateTime.TryParse(expiresAt, out var result) ? result : null;
        }

        public string? GetTokenInfo(string tokenInfo)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            if (session is null)
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