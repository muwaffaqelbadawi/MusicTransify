using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Configurations.Spotify
{
    // POCO (Plain Old CLR Object) Entities
    // Represents the configuration options for Spotify API
    public class SpotifyOptions
    {
        [JsonPropertyName("client_name")]
        public string ClientName { get; set; } = string.Empty;
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = string.Empty;
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = string.Empty;
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; } = string.Empty;
        [JsonPropertyName("auth_uri")]
        public string AuthUri { get; set; } = string.Empty;
        [JsonPropertyName("token_uri")]
        public string TokenUri { get; set; } = string.Empty;
        [JsonPropertyName("api_base_uri")]
        public string ApiBaseUri { get; set; } = string.Empty;
        [JsonPropertyName("api_version")]
        public string GrantType { get; set; } = string.Empty;
        [JsonPropertyName("refresh_token_grant_type")]
        public string RefreshTokenGrantType { get; set; } = string.Empty;
        [JsonPropertyName("response_type")]
        public string ResponseType { get; set; } = string.Empty;
        [JsonPropertyName("show_dialog")]
        public string ShowDialog { get; set; } = string.Empty;
        [JsonPropertyName("playlist_uri")]
        public string PlaylistUri { get; set; } = string.Empty;
        [JsonPropertyName("scope")]
        public string[] Scope { get; set; } = Array.Empty<string>();
    }
}