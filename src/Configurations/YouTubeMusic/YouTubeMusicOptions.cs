using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Configurations.YouTubeMusic
{
    // POCO (Plain Old CLR Object) Entities
    // Represents the configuration options for YouTube Music API
    public class YouTubeMusicOptions
    {
        [JsonPropertyName("client_name")]
        public string ClientName { get; set; } = string.Empty;
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = string.Empty;
        [JsonPropertyName("client_email")]
        public string RedirectUri { get; set; } = string.Empty;
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = string.Empty;
        [JsonPropertyName("project_id")]
        public string ProjectId { get; set; } = string.Empty;
        [JsonPropertyName("auth_uri")]
        public string AuthUri { get; set; } = string.Empty;
        [JsonPropertyName("token_uri")]
        public string TokenUri { get; set; } = string.Empty;
        [JsonPropertyName("auth_provider_x509_cert_uri")]
        public string AuthProviderX509CertUri { get; set; } = string.Empty;
        [JsonPropertyName("api_base_uri")]
        public string ApiBaseUri { get; set; } = string.Empty;
        [JsonPropertyName("response_type")]
        public string ResponseType { get; set; } = string.Empty;
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = string.Empty;
        [JsonPropertyName("refresh_token_grant_type")]
        public string RefreshTokenGrantType { get; set; } = string.Empty;
        [JsonPropertyName("access_type")]
        public string AccessType { get; set; } = string.Empty;
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;
        [JsonPropertyName("playlist_endpoint")]
        public string PlaylistUri { get; set; } = string.Empty;
        [JsonPropertyName("scope")]
        public string[] Scope { get; set; } = Array.Empty<string>();
        [JsonPropertyName("include_granted_scopes")]
        public string IncludeGrantedScopes { get; set; } = string.Empty;
    }
}