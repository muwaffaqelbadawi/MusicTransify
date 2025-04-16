using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.utilities
{
    public class AuthHelper
    {
        readonly HttpContext _context;

        public AuthHelper(HttpContext context)
        {
            _context = context;
        }



        // Helper function for building query strings
        public string ToQueryString(object queryParameters)
        {
            return string.Join("&",
            queryParameters.GetType().GetProperties()
            .Select(prop => $"{prop.Name}={Uri.EscapeDataString(prop.GetValue(queryParameters)?.ToString() ?? string.Empty)}"));
        }

        // Helper function for creating time stamp token expiration date
        public long ToTimeStamp(string strExpiresIn)
        {
            // Set ExpiresIn
            long ExpiresIn = long.Parse(strExpiresIn);

            // Converting expiration date to Unix time stamp
            return DateTimeOffset.UtcNow.AddSeconds(ExpiresIn).ToUnixTimeSeconds(); ;
        }

        // Helper function to check token expiry
        public bool IsTokenExpired()
        {
            // Set and check expires_in is not null
            string OldStrExpiresIn = _context.Session.GetString("expires_in") ?? throw new InvalidOperationException("No 'refresh_token' found");

            // Set OldExpiresIn
            long ExpiresIn = long.Parse(OldStrExpiresIn);

            // Set the current time
            long CurrentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Check if the token is expired
            return CurrentTime > ExpiresIn;
        }
    }
}