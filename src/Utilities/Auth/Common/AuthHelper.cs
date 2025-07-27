using System;

namespace MusicTransify.src.Utilities.Auth.Common
{
    public class AuthHelper
    {
        public string ToQueryString(Dictionary<string, string> queryParameters)
        {
            ArgumentNullException.ThrowIfNull(queryParameters);

            return string.Join("&", queryParameters
                .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
                .Select(kvp =>
                    $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"
            ));
        }

        public string BuildScopeString(string[] scopes)
        {
            if (scopes is null || scopes.Length == 0) return string.Empty;

            var distinctScopes = scopes.Distinct();

            return string.Join(" ", distinctScopes);
        }

        public string FormRedirectUrl(string authUri, string queryString)
        {
            if (string.IsNullOrEmpty(authUri))
            {
                throw new InvalidOperationException("AuthUri is not configured.");
            }

            if (string.IsNullOrEmpty(queryString))
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            return $"{authUri}?{queryString}";
        }
    }
}