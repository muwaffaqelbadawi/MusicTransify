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
                return string.Empty;
            }

            return string.Join("&",
            queryParameters.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.GetIndexParameters().Length == 0)
            .Select(prop =>
            {
                var key = Uri.EscapeDataString(prop.Name); // Encode property name
                var value = Uri.EscapeDataString(prop.GetValue(queryParameters)?.ToString() ?? string.Empty); // Encode property value
                return $"{key}={value}";
            }));
        }
    }
}