using System;
using System.Linq;
using System.Security.Cryptography;

namespace MusicTransify.src.Utilities.Security
{
    public class AuthHelper
    {
        // Helper function for building query strings from dictionary
        public string ToQueryString(Dictionary<string, string> queryParameters)
        {
            if (queryParameters is null || queryParameters.Count == 0) return string.Empty;

            var encodedParams = queryParameters
            .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
            .Select(kvp =>
            $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}");

            return string.Join("&", encodedParams);
        }

        // Helper function to generate random string
        public string GenerateSecureRandomString(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
            using (var rng = RandomNumberGenerator.Create())
            {
                var byteBuffer = new byte[length];
                rng.GetBytes(byteBuffer);

                var chars = new char[length];
                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[byteBuffer[i] % validChars.Length];
                }
                return new string(chars);
            }
        }
    }
}