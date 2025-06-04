using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Models.Spotify
{
    public class SpotifyCallback
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("grant_type")]
        public string? GrantType { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public int Scope { get; set; }
    }
}