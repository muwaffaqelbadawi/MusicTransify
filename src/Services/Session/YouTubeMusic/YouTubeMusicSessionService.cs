using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using MusicTransify.src.Api.Endpoints.DTOs.Responses.Token.YouTubeMusic;
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

        public class TokenData
        {
            public string AccessToken { get; set; } = default!;
            public string TokenType { get; set; } = default!;
            public int ExpiresIn { get; set; }
            public string? RefreshToken { get; set; }
            public string Scope { get; set; } = default!;
        }

        #region State Management
        public void StoreState(string state)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            httpContext?.Response.Cookies.Append("YouTubeMusicState", state, new CookieOptions
            {
                HttpOnly = true, // Cookie is not accessible via JavaScript
                Secure = false, // Set to true if using HTTPS
                SameSite = SameSiteMode.None, // Cookie is sent with cross-site requests
                IsEssential = true, // Cookie is essential for the application to function
                Expires = DateTimeOffset.UtcNow.AddMinutes(30) // Cookie expiration time
            });
        }

        public bool ValidateState(string state)
        {
            var httpContext = GetValidHttpContext();
            var storedState = httpContext?.Request.Cookies["YouTubeMusicState"];
            if (storedState == state)
            {
                httpContext?.Response.Cookies.Delete("YouTubeMusicState");
                return true;
            }
            return false;
        }
        #endregion

        #region Token Management
        public TokenData ExtractTokenData(
            YouTubeMusicTokenResponseDto tokenInfo,
            bool requireRefreshToken = false
        )
        {
            if (tokenInfo is null)
            {
                _logger.LogError("Token information is missing from response");
                throw new ArgumentNullException(nameof(tokenInfo), "Token information is missing from response");
            }

            if (string.IsNullOrEmpty(tokenInfo.AccessToken))
            {
                _logger.LogError("Access token is missing from response");
                throw new ArgumentNullException(nameof(tokenInfo.AccessToken), "Access token is missing from response");
            }

            if (string.IsNullOrEmpty(tokenInfo.TokenType))
            {
                _logger.LogError("Token type is missing from response");
                throw new ArgumentNullException(nameof(tokenInfo.TokenType), "Token type is missing from response");
            }

            if (requireRefreshToken && string.IsNullOrEmpty(tokenInfo.RefreshToken))
            {
                _logger.LogError("Refresh token is missing from response");
                throw new ArgumentNullException(nameof(tokenInfo.RefreshToken), "Refresh token is missing from response");
            }

            if (string.IsNullOrEmpty(tokenInfo.Scope))
            {
                _logger.LogError("Scope is missing from response");
                throw new ArgumentNullException(nameof(tokenInfo.Scope), "Scope is missing from response");
            }

            int expiresIn = tokenInfo.ExpiresIn > 0 ? tokenInfo.ExpiresIn : 3600;

            return new TokenData
            {
                // Here's the problem
                AccessToken = tokenInfo.AccessToken,
                TokenType = tokenInfo.TokenType,
                ExpiresIn = expiresIn,
                RefreshToken = tokenInfo.RefreshToken,
                Scope = tokenInfo.Scope
            };
        }
        #endregion

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

        private HttpContext GetValidHttpContext()
        {
            return _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP Context is not available");
        }

        #region Session Management
        public void Store(YouTubeMusicTokenResponseDto tokenInfo)
        {
            var tokenData = ExtractTokenData(tokenInfo, requireRefreshToken: false);

            if (tokenData.RefreshToken is null)
            {
                _logger.LogWarning("No refresh token provided");
            }

            var session = GetValidSession();
            string newExpiresIn = _tokenHelper.CalculateExpirationDate(tokenData.ExpiresIn);

            if (session is null)
            {
                _logger.LogError("Session is not available");
                throw new InvalidOperationException("Session is not available");
            }

            _logger.LogInformation("AccessToken length: {Length}, Scope: {Scope}, Expires in: {ExpiresIn}s",
            tokenData.AccessToken.Length, tokenData.Scope, tokenData.ExpiresIn);

            // What the hell is happening here!!!
            session.SetString("access_token", tokenData.AccessToken);

            // Immediate call works!!!!!!
            _logger.LogInformation(
                "This is the DEBUG I JUST WROTE Access token: {access token} and session ID: {sessionId}",
                session.GetString("access_token"), session.Id
            );

            session.SetString("token_type", tokenData.TokenType);

            if (!string.IsNullOrEmpty(tokenData.RefreshToken))
            {
                session.SetString("refreshToken", _protector.Protect(tokenData.RefreshToken));
            }

            session.SetString("expires_in", newExpiresIn);
            session.SetString("scope", tokenData.Scope);


            _logger.LogInformation("Successfully stored token info in session");
        }
        #endregion

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

                "refreshToken" => session.GetString("refreshToken"),

                "expires_in" => session.GetString("expires_in"),

                "scope" => session.GetString("scope"),

                _ => throw new ArgumentException($"Invalid tokenInfo: {tokenInfo}", nameof(tokenInfo))
            };
        }
    }
}
