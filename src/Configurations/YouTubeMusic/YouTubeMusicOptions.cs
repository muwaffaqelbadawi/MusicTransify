using System;

namespace MusicTransify.src.Configurations.YouTubeMusic
{
    // POCO (Plain Old CLR Object) Entities
    // Represents the configuration options for YouTube Music API
    public class YouTubeMusicOptions
    {
        public string ClientName { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public string AuthUri { get; set; } = string.Empty;
        public string TokenUri { get; set; } = string.Empty;
        public string AuthProviderX509CertUri { get; set; } = string.Empty;
        public string ApiBaseUri { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;
        public string RefreshTokenGrantType { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
        public string PlaylistUrl { get; set; } = string.Empty;
        public string[] Scope { get; set; } = Array.Empty<string>();
        public string IncludeGrantedScopes { get; set; } = string.Empty;
    }
}