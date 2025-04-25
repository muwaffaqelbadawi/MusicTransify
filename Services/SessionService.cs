using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using SpotifyWebAPI_Intro.utilities;

namespace SpotifyWebAPI_Intro.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthHelper _authHelper;
        private readonly TokenHelper _tokenHelper;

        public SessionService(IHttpContextAccessor httpContextAccessor, AuthHelper authHelper, TokenHelper tokenHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _authHelper = authHelper;
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

            string AccessToken = Assets.AccessToken;
            string RefreshToken = Assets.RefreshToken;
            string _ExpiresIn = Assets.ExpiresIn;

            // Calculate token expiration date
            string ExpiresIn = _tokenHelper.CalculateExpirationDate(_ExpiresIn);

            // Initialize session storage
            var Session = _httpContextAccessor.HttpContext?.Session
            ?? throw new InvalidOperationException("HttpContext or Session is null.");

            // Store access token in session
            Session.SetString("access_token", AccessToken);

            // Store refresh token in session
            Session.SetString("refresh_token", RefreshToken);

            // Store expiration date in session
            Session.SetString("expires_in", ExpiresIn);
        }

        // Expose token information from session
        public string GetTokenInfo(string TokenInfo)
        {
            var Session = _httpContextAccessor.HttpContext?.Session
            ?? throw new InvalidOperationException("HttpContext or Session is null.");

            // Check the requested TokenInfo and return the appropriate session value
            return TokenInfo switch
            {
                "AccessToken" => Session.GetString("access_token") ?? "Access token not found.",
                "RefreshToken" => Session.GetString("refresh_token") ?? "Refresh token not found.",
                "ExpiresIn" => Session.GetString("expires_in") ?? "Expiration time not found.",
                _ => throw new ArgumentException($"Invalid TokenInfo: {TokenInfo}", nameof(TokenInfo))
            };
        }

        public static implicit operator SessionService(SessionOptions v)
        {
            throw new NotImplementedException();
        }
    }
}
