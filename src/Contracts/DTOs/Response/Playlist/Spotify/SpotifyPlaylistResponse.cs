using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Contracts.DTOs.Response.Playlist.Spotify
{
    public class SpotifyPlaylistResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("images")]
        public List<SpotifyImage> Images { get; set; } = new();

        [JsonPropertyName("owner")]
        public SpotifyOwner Owner { get; set; } = new();

        [JsonPropertyName("public")]
        public bool Public { get; set; }

        [JsonPropertyName("tracks")]
        public SpotifyTrackLink Tracks { get; set; } = new();

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("uri")]
        public string Uri { get; set; } = string.Empty;
    }

    public class SpotifyImage
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }
    }

    public class SpotifyOwner
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;
    }

    public class SpotifyTrackLink
    {
        [JsonPropertyName("href")]
        public string Href { get; set; } = string.Empty;

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}