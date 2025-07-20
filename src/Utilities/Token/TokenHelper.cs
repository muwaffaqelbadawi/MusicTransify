using System;

namespace MusicTransify.src.Utilities.Token
{
    public class TokenHelper
    {
        public long ToTimeStamp(long seconds)
        {
            return DateTimeOffset.UtcNow.AddSeconds(seconds).ToUnixTimeSeconds();
        }

        public string CalculateExpirationDate(long OldExpiresIn)
        {
            long expiresIn = ToTimeStamp(OldExpiresIn);
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long newExpiresIn = currentTime + expiresIn;
            return newExpiresIn.ToString();
        }

        public bool IsExpired(long expiresIn)
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return currentTime > expiresIn;
        }

        public long ParseToLong(string strExpiresIn)
        {
            if (!long.TryParse(strExpiresIn, out long expiresIn))
            {
                throw new InvalidOperationException("The parameter expires_in is not found");
            }
            return expiresIn;
        }
    }
}