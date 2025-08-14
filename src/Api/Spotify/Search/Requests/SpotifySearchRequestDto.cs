using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Api.Spotify.Search.Requests
{
    public class SpotifySearchRequestDto
    {
        // Search query (keywords, URIs, etc.). (default required)
        [JsonPropertyName("q")]
        public string? Q { get; set; }

        // playlist for playlists search. (default required)
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        // Filter by availability. (default None)
        [JsonPropertyName("market")]
        public string? Market { get; set; }

        //  Max results per request. (default 20)
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        // Pagination index. (default 0)
        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        // Optional: include external audio results
        [JsonPropertyName("include_external")]
        public string? IncludeExternal { get; set; } // e.g., "audio"

        public Dictionary<string, string> ToMap()
        {
            var map = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(Q)) map["q"] = Q;
            if (!string.IsNullOrEmpty(Type)) map["type"] = Type;
            if (!string.IsNullOrEmpty(Market)) map["market"] = Market;
            if (Limit.HasValue) map["limit"] = Limit.Value.ToString();
            if (Offset.HasValue) map["offset"] = Offset.Value.ToString();
            if (!string.IsNullOrEmpty(IncludeExternal)) map["include_external"] = IncludeExternal;

            return map;
        }
    }
}