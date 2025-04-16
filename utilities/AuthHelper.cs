using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotifyWebAPI_Intro.Services;

namespace SpotifyWebAPI_Intro.utilities
{
    public class AuthHelper
    {
        public AuthHelper()
        {

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
            return DateTimeOffset.UtcNow.AddSeconds(ExpiresIn).ToUnixTimeSeconds();
        }

        // Helper function to check token expiry
        public bool IsExpired(string OldExpiresIn)
        {
            // Set OldExpiresIn
            long ExpiresIn = long.Parse(OldExpiresIn);

            // Set the current time
            long CurrentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Check if the token is expired
            return CurrentTime > ExpiresIn;
        }
    }
}