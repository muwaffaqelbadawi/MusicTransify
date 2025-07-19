using System;

namespace MusicTransify.src.Utilities.Token
{
    public class TokenHelper
    {
        // Helper function for creating time stamp token expiration date
        public long ToTimeStamp(long seconds)
        {
            // Converting expiration date to Unix time stamp
            return DateTimeOffset.UtcNow.AddSeconds(seconds).ToUnixTimeSeconds();
        }

        public string CalculateExpirationDate(long OldExpiresIn)
        {
            // Set token expiration date
            long expiresIn = ToTimeStamp(OldExpiresIn);

            // Set current time
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Calculate the new expiration date
            long newExpiresIn = currentTime + expiresIn;

            // return token expiration data as a string
            return newExpiresIn.ToString();
        }

        // Helper function to check token expiry
        public bool IsExpired(long expiresIn)
        {
            // Set the current time
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Check if the token is expired
            return currentTime > expiresIn;
        }

        public long ParseToLong(string strExpiresIn)
        {
            // Check the present of expires in
            if (!long.TryParse(strExpiresIn, out long expiresIn))
            {
                throw new InvalidOperationException("The parameter expires_in is not found");
            }
            return expiresIn;
        }
    }
}