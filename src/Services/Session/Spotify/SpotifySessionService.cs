using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using MusicTransify.src.Contracts.DTOs.Response.Token.Spotify;
using MusicTransify.src.Contracts.Session.Spotify;
using MusicTransify.src.Utilities.Token;

namespace MusicTransify.src.Services.Session.Spotify
{
    public class SpotifySessionService : ISpotifySessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenHelper _tokenHelper;
        private readonly IDataProtector _protector;
        private readonly ILogger<SpotifySessionService> _logger;

        public SpotifySessionService(
            IHttpContextAccessor httpContextAccessor,
            TokenHelper token,
            IDataProtectionProvider protectionProvider,
            ILogger<SpotifySessionService> logger
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenHelper = token;
            _protector = protectionProvider.CreateProtector("TokenProtection");
            _logger = logger;
        }

        #region State Management
        // 1
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

        // 2
        public bool ValidateState(string state)
        {
            var httpContext = GetValidHttpContext();
            var storedState = httpContext?.Request.Cookies["SpotifyState"];
            return storedState == state;
        }
        #endregion

        #region Token Management

        // 3
        public (
            string accessToken,
            string tokenType,
            int expiresIn,
            string refreshToken,
            string scope
        )
        ExtractTokenData(SpotifyTokenResponse tokenInfo)
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

        // 4
        public void Store(SpotifyTokenResponse tokenInfo)
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

            _logger.LogInformation("AccessToken length: {Length}, Scope: {Scope}, Expires in: {ExpiresIn}s",
            accessToken.Length, scope, expiresIn);

            session.SetString("access_token", accessToken);
            session.SetString("token_type", tokenType);

            if (!string.IsNullOrEmpty(refreshToken))
            {
                session.SetString("refreshToken", _protector.Protect(refreshToken));
            }

            session.SetString("expires_in", newExpiresIn);
            session.SetString("scope", scope);
            _logger.LogInformation("Successfully stored token info in session");
        }

        // 5
        public void RegenerateSession()
        {
            var session = GetValidSession();
            session.Clear();
            session.CommitAsync().Wait();
        }

        // 6
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

        // 7
        public string? GetProtectedToken(string tokenKey)
        {
            var session = _httpContextAccessor.HttpContext?.Session
                ?? throw new InvalidOperationException("Session unavailable");

            var encryptedValue = session.GetString(tokenKey);
            return !string.IsNullOrEmpty(encryptedValue)
                ? _protector.Unprotect(encryptedValue)
                : null;
        }

        // 8
        public string? GetAccessToken() => GetProtectedToken("access_token");

        // 9
        public string? GetRefreshToken() => GetProtectedToken("refreshToken");

        // 10
        public DateTime? GetExpiration()
        {
            var expiresAt = _httpContextAccessor.HttpContext?.Session.GetString("expires_at");
            return DateTime.TryParse(expiresAt, out var result)
            ? result
            : null;
        }

        // 11
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

                "refreshToken" => session.GetString("refreshToken"),

                "expires_in" => session.GetString("expires_in"),

                "scope" => session.GetString("scope"),

                _ => throw new ArgumentException($"Invalid tokenInfo: {tokenInfo}", nameof(tokenInfo))
            };
        }
        #endregion

        #region Private Helpers

        // Not include in the interface
        private HttpContext GetValidHttpContext()
        {
            return _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP Context is not available");
        }

        // Not include in the interface
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