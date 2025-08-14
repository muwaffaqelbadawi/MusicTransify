using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;

namespace MusicTransify.src.Api.Spotify.Playlists.Requests
{
    public class SpotifyPrivatePlaylistsRequestDto
    {
        // Get a User’s private Playlists
        public class SpotifyPublicPlaylistsRequestDto : IMappable
        {
            //  Max results per request. (default 20)
            [JsonPropertyName("limit")]
            public int? Limit { get; set; }

            // Pagination index. (default 0)
            [JsonPropertyName("offset")]
            public int? Offset { get; set; }

            // e.g., "track,episode" (default None)
            [JsonPropertyName("additional_types")]
            public string? AdditionalTypes { get; set; }

            public Dictionary<string, string> ToMap()
            {
                var map = new Dictionary<string, string>();

                if (Limit.HasValue) map["limit"] = Limit.Value.ToString();
                if (Offset.HasValue) map["offset"] = Offset.Value.ToString();
                if (!string.IsNullOrEmpty(AdditionalTypes)) map["additional_types"] = AdditionalTypes;

                return map;
            }
        }
    }
}