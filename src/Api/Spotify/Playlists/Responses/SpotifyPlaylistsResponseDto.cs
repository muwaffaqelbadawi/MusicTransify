using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Api.Spotify.Playlists.Responses
{
    public class SpotifyPlaylistsResponseDto
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("images")]
        public List<SpotifyImage>? Images { get; set; }

        [JsonPropertyName("owner")]
        public SpotifyOwner? Owner { get; set; }

        [JsonPropertyName("public")]
        public bool Public { get; set; }

        [JsonPropertyName("tracks")]
        public SpotifyTrackLink? Tracks { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
    }

    public partial class SpotifyImage
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }
    }

    public partial class SpotifyOwner
    {
        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }
    }

    public partial class SpotifyTrackLink
    {
        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}