using System;
using System.Reflection;
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
            if (queryParameters == null)
            {
                return string.Empty; // Handle null input gracefully
            }

            return string.Join("&",
            queryParameters.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance) // Only public instance properties
            .Where(prop => prop.GetIndexParameters().Length == 0) // Exclude indexed properties
            .Select(prop =>
            {
                var key = Uri.EscapeDataString(prop.Name); // Encode property name
                var value = Uri.EscapeDataString(prop.GetValue(queryParameters)?.ToString() ?? string.Empty); // Encode property value
                return $"{key}={value}";
            }));
        }

        // Helper function for creating time stamp token expiration date
        public long ToTimeStamp(string strExpiresIn)
        {
            // Set ExpiresIn
            long ExpiresIn = long.Parse(strExpiresIn);

            // Converting expiration date to Unix time stamp
            return DateTimeOffset.UtcNow.AddSeconds(ExpiresIn).ToUnixTimeSeconds();
        }

        public string CalculateTokenExpirationDate(string StrExpiresIn)
        {
            // Set token expiration date
            long ExpiresIn = ToTimeStamp(StrExpiresIn);

            // Check if the token is expired
            if (IsExpired(StrExpiresIn))
            {
                // Set Current time
                long CurrentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                // Calculate token expiration date
                long NewExpiresIn = CurrentTime + ExpiresIn;

                // return new token expiration date as a string
                return NewExpiresIn.ToString();
            }

            // return token expiration data as a string
            return ExpiresIn.ToString();
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