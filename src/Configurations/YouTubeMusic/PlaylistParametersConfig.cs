using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Configurations.YouTubeMusic
{
    public class PlaylistParametersConfig
    {
        [JsonPropertyName("part")]
        public string? Part { get; set; }

        [JsonPropertyName("mine")]
        public string? Mine { get; set; }
    }
}