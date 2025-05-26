using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace SpotifyWebAPI_Intro.src.Models.YouTubeMusic
{
    public class YouTubeMusicCallback
    {
        [JsonPropertyName("")]
        public string? Code { get; set; }

        [JsonPropertyName("")]
        public string? Error { get; set; }

        [JsonPropertyName("")]
        public string? GrantType { get; set; }

        [JsonPropertyName("")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public int Scope { get; set; }
    }
}