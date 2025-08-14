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

        public string BuildRedirectUri(string authUri, string queryString)
        {
            if (string.IsNullOrEmpty(authUri))
            {
                throw new InvalidOperationException("AuthUri is not configured.");
            }

            if (string.IsNullOrEmpty(queryString))
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            // Structure: https://{apiBaseUri}?{queryString}
            // Structure: https://accounts.spotify.com/authorize?{queryString}
            return $"{authUri}?{queryString}";
        }

        public string BuildApiUri(
            string apiBaseUri,
            string endpoint,
            string queryString
        )
        {
            if (string.IsNullOrEmpty(apiBaseUri))
            {
                throw new InvalidOperationException($"apiBaseUri is not configured. {nameof(apiBaseUri)}");
            }

            if (string.IsNullOrEmpty(endpoint))
            {
                throw new InvalidOperationException($"endpoint is not configured. {nameof(endpoint)}");
            }

            if (string.IsNullOrEmpty(queryString))
            {
                throw new ArgumentNullException($"endpoint is not configured. {nameof(queryString)}");
            }

            // Structure: https://{apiBaseUri}/{endpoint}?{queryString}
            // Structure: https://https://api.spotify.com/v1/{endpoint}?{queryString}
            return $"{apiBaseUri}/{endpoint}?{queryString}";
        }
    }
}