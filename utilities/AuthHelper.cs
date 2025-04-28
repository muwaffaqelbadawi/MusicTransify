using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace SpotifyWebAPI_Intro.utilities
{
    public class AuthHelper
    {
        // Helper function for building query strings from dictionary
        public string ToQueryString(Dictionary<string, string> queryParameters)
        {
            if (queryParameters == null || queryParameters.Count == 0)
                return string.Empty;

            var encodedParams = queryParameters
            .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
            .Select(kvp =>
            $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}");

            return string.Join("&", encodedParams);
        }

        // Helper function to generate random string
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}