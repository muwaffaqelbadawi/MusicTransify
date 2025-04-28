using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using SpotifyWebAPI_Intro.utilities;

namespace SpotifyWebAPI_Intro.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenHelper _tokenHelper;

        public SessionService(IHttpContextAccessor httpContextAccessor, TokenHelper tokenHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenHelper = tokenHelper;
        }

        // Check query string existence
        public (string AccessToken, string RefreshToken, string ExpiresIn) Check(JsonElement TokenInfo)
        {
            // Set and check access_token is not null
            string AccessToken = TokenInfo.GetString("access_token") ?? throw new InvalidOperationException("No 'access_token' found");

            // Set and check refresh_token is not null
            string RefreshToken = TokenInfo.GetString("refresh_token") ?? throw new InvalidOperationException("No 'refresh_token' found");

            // Set and check expires_in is not null
            string ExpiresIn = TokenInfo.GetString("expires_in") ?? throw new InvalidOperationException("No 'refresh_token' found");

            return (AccessToken, RefreshToken, ExpiresIn);
        }

        // Store Token Info in session
        public void Store(JsonElement TokenInfo)
        {
            // Check existence of token assets
            var Assets = Check(TokenInfo);

            // Initialize session storage
            var Session = _httpContextAccessor.HttpContext?.Session
            ?? throw new InvalidOperationException("HttpContext or Session is null.");

            // Optionally, pass the old expiration date if you want to update it only if expired
            string ExpiresIn = _tokenHelper.CalculateExpirationDate(Assets.ExpiresIn, Session.GetString("expires_in"));

            // Store access token in session
            Session.SetString("access_token", Assets.AccessToken);

            // Store refresh token in session
            Session.SetString("refresh_token", Assets.RefreshToken);

            // Store expiration date in session
            Session.SetString("expires_in", ExpiresIn);
        }

        // Expose token information from session
        public string GetTokenInfo(string TokenInfo)
        {
            var Session = _httpContextAccessor.HttpContext?.Session
            ?? throw new InvalidOperationException("HttpContext or Session is null.");

            return TokenInfo switch
            {
                "AccessToken" => Session.GetString("access_token") ?? "Access token not found.",
                "RefreshToken" => Session.GetString("refresh_token") ?? "Refresh token not found.",
                "ExpiresIn" => Session.GetString("expires_in") ?? "Expiration time not found.",
                _ => throw new ArgumentException($"Invalid TokenInfo: {TokenInfo}", nameof(TokenInfo))
            };
        }
    }
}
