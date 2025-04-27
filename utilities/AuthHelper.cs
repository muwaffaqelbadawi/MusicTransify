using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace SpotifyWebAPI_Intro.utilities
{
    public class AuthHelper
    {
        // Helper function for building query strings
        public string ToQueryString(Dictionary<string, string> QueryParameters)
        {
            if (QueryParameters == null || QueryParameters.Count == 0) return string.Empty;

            var EncodedParams = QueryParameters
            .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
            .Select(kvp =>
            $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}");

            return EncodedParams.Any()
            ? "?" + string.Join("&", EncodedParams)
            : string.Empty;
        }
    }
}