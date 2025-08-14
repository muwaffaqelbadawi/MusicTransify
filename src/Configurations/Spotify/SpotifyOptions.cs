using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Configurations.Spotify
{
    public class SpotifyOptions
    {
        [JsonPropertyName("client_name")]
        public string? ClientName { get; set; }
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }
        [JsonPropertyName("redirect_uri")]
        public string? RedirectUri { get; set; }
        [JsonPropertyName("auth_uri")]
        public string? AuthUri { get; set; }
        [JsonPropertyName("token_uri")]
        public string? TokenUri { get; set; }
        [JsonPropertyName("api_base_uri")]
        public string? ApiBaseUri { get; set; }
        [JsonPropertyName("grant_type")]
        public string? GrantType { get; set; }
        [JsonPropertyName("refresh_token_grant_type")]
        public string? RefreshTokenGrantType { get; set; }
        [JsonPropertyName("response_type")]
        public string? ResponseType { get; set; }
        [JsonPropertyName("show_dialog")]
        public string? ShowDialog { get; set; }
        [JsonPropertyName("scope")]
        public string[]? Scope { get; set; } = Array.Empty<string>();
    }
}