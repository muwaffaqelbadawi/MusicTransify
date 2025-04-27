using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.utilities
{
    public class TokenHelper
    {
        // Helper function for creating time stamp token expiration date
        public long ToTimeStamp(string ExpiresIn)
        {
            // Set ExpiresIn
            long Seconds = long.Parse(ExpiresIn);

            // Converting expiration date to Unix time stamp
            return DateTimeOffset.UtcNow.AddSeconds(Seconds).ToUnixTimeSeconds();
        }

        public string CalculateExpirationDate(string ExpiresIn, string? OldExpiresIn = null)
        {
            // Set token expiration date
            long NewExpiresIn = ToTimeStamp(ExpiresIn);

            // Optionally check for old expiration date
            if (!string.IsNullOrEmpty(OldExpiresIn) && IsExpired(OldExpiresIn))
            {
                long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                long expiresInValue = currentTime + NewExpiresIn;
                return expiresInValue.ToString();
            }

            // return token expiration data as a string
            return ExpiresIn.ToString();
        }

        // Helper function to check token expiry
        public bool IsExpired(string ExpiresIn)
        {
            long Expiration = long.Parse(ExpiresIn);

            // Set the current time
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Check if the token is expired
            return currentTime > Expiration;
        }
    }
}