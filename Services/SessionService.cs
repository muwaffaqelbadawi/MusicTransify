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
        readonly HttpContext _context;
        private readonly AuthHelper _authHelper;

        public SessionService(HttpContext context, AuthHelper authHelper)
        {
            _context = context;
            _authHelper = authHelper;
        }

        // Check query string existence
        public (string AccessToken, string RefreshToken, string ExpiresIn) CheckAssets(JsonElement TokenInfo)
        {
            // Set and check access_token is not null
            string AccessToken = TokenInfo.GetString("access_token") ?? throw new InvalidOperationException("No 'access_token' found");

            // Set and check refresh_token is not null
            string RefreshToken = TokenInfo.GetString("refresh_token") ?? throw new InvalidOperationException("No 'refresh_token' found");

            // Set and check expires_in is not null
            string ExpiresIn = TokenInfo.GetString("expires_in") ?? throw new InvalidOperationException("No 'refresh_token' found");

            return (AccessToken, RefreshToken, ExpiresIn);
        }

        public string CalculateTokenExpirationDate(string StrExpiresIn)
        {
            // Set token expiration date
            long ExpiresIn = _authHelper.ToTimeStamp(StrExpiresIn);

            // returning token expiration data as a string
            return ExpiresIn.ToString();
        }



        // Calculate refresh token expiration date
        public string CalculateRefreshTokenExpirationDate(string StrExpiresIn)
        {
            // Set NewExpiresIn
            long OldExpiresIn = _authHelper.ToTimeStamp(StrExpiresIn);

            // Set Current time
            long CurrentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Calculate token expiration date
            long ExpiresIn = CurrentTime + OldExpiresIn;

            // returning token expiration data as a string
            return ExpiresIn.ToString();
        }

        // Store Token Info in session
        public void StoreAssetes(string AccessToken, string RefreshToken, string ExpiresIn)
        {
            // Store access token in session
            _context.Session.SetString("access_token", AccessToken);

            // Store refresh token in session
            _context.Session.SetString("refresh_token", RefreshToken);

            // Store expiration date in session
            _context.Session.SetString("expires_in", ExpiresIn);
        }
    }
}