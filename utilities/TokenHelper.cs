using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SpotifyWebAPI_Intro.Services;

namespace SpotifyWebAPI_Intro.utilities
{
    public class TokenHelper
    {
        private readonly SessionService _sessionService;
        public TokenHelper(SessionOptions sessionService)
        {
            _sessionService = sessionService;
        }

        // Helper function for creating time stamp token expiration date
        public long ToTimeStamp(string _ExpiresIn)
        {
            // Set ExpiresIn
            long ExpiresIn = long.Parse(_ExpiresIn);

            // Converting expiration date to Unix time stamp
            return DateTimeOffset.UtcNow.AddSeconds(ExpiresIn).ToUnixTimeSeconds();
        }

        public string CalculateExpirationDate(string _ExpiresIn)
        {
            // Set token expiration date
            long __ExpiresIn = ToTimeStamp(_ExpiresIn);

            // Check if the token is expired
            if (IsExpired())
            {
                // Set Current time
                long CurrentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                // Calculate token expiration date
                long ExpiresIn = CurrentTime + __ExpiresIn;

                // return new token expiration date as a string
                return ExpiresIn.ToString();
            }

            // return token expiration data as a string
            return _ExpiresIn.ToString();
        }

        // Helper function to check token expiry
        public bool IsExpired()
        {
            // Set OldExpiresIn
            string _ExpiresIn = _sessionService.GetTokenInfo("ExpiresIn");
            long ExpiresIn = long.Parse(_ExpiresIn);

            // Set the current time
            long CurrentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Check if the token is expired
            return CurrentTime > ExpiresIn;
        }
    }
}