using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;

namespace MusicTransify.src.Api.Spotify.Playlists.Requests
{
    // Get a User’s public Playlists
    public class SpotifyPublicPlaylistsRequestDto : IMappable 
    {
        //  Max results per request. (Default 20 Max 20)
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        // Pagination index. (Default 0)
        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        public Dictionary<string, string> ToMap()
        {
            var map = new Dictionary<string, string>();

            if (Limit.HasValue) map["limit"] = Limit.Value.ToString();
            if (Offset.HasValue) map["offset"] = Offset.Value.ToString();

            return map;
        }
    }
}