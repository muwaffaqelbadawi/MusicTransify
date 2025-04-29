using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using SpotifyWebAPI_Intro.Utilities;

namespace SpotifyWebAPI_Intro.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenHelper _token;
        private readonly ILogger<SessionService> _logger;

        public SessionService(IHttpContextAccessor httpContextAccessor, TokenHelper token, ILogger<SessionService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _token = token;
            _logger = logger;
        }

        // Extracts token fields from the Spotify token JSON response
        public (string accessToken, string tokenType, int expiresIn, string refreshToken, string scope) Check(JsonElement tokenInfo)
        {
            try
            {
                // Set and check access_token is not null
                string accessToken = tokenInfo.GetProperty("access_token").GetString() ?? throw new InvalidOperationException("No 'access_token' found");

                // Set and check access_token is not null
                string tokenType = tokenInfo.GetProperty("token_type").GetString() ?? throw new InvalidOperationException("No 'token_type' found");

                // Set and check refresh_token is not null
                int expiresIn = tokenInfo.GetProperty("expires_in").GetInt32();

                // Set and check expires_in is not null
                string refreshToken = tokenInfo.GetProperty("refresh_token").GetString() ?? throw new InvalidOperationException("No 'refresh_token' found");

                // Set and check expires_in is not null
                string scope = tokenInfo.GetProperty("scope").GetString() ?? throw new InvalidOperationException("No 'scope' found");

                return (accessToken, tokenType, expiresIn, refreshToken, scope);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to extract tokens from tokenInfo: {TokenInfo}", tokenInfo.ToString());
                throw;
            }
        }

        // Store Token Information in session
        public void Store(JsonElement tokenInfo)
        {
            // Check existence of token assets
            var (accessToken, tokenType, expiresIn, refreshToken, scope) = Check(tokenInfo);

            // Initialize session storage
            var Session = _httpContextAccessor.HttpContext?.Session
            ?? throw new InvalidOperationException("HttpContext or Session is null.");

            // Optionally, pass the old expiration date if you want to update it only if expired
            string newExpiresIn = _token.CalculateExpirationDate(expiresIn);

            // Store access token in session
            Session.SetString("access_token", accessToken);

            // Store token type in session
            Session.SetString("token_type", tokenType);

            // Store refresh token in session
            Session.SetString("refresh_token", refreshToken);

            // Store expiration date in session
            Session.SetString("expires_in", newExpiresIn);

            // Store expiration date in session
            Session.SetString("scope", scope);
        }

        // Gets token information from session. Returns null if the value is not found.
        public string? GetTokenInfo(string tokenInfo)
        {
            var session = _httpContextAccessor.HttpContext?.Session
            ?? throw new InvalidOperationException("HttpContext or Session is null.");

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
    }
}
