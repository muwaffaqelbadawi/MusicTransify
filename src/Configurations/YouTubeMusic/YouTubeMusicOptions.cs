using System;

namespace MusicTransify.src.Configurations.YouTubeMusic
{
    public class YouTubeMusicOptions
    {
        public string? ClientName { get; set; }
        public string? ClientSecret { get; set; }
        public string? RedirectUri { get; set; }
        public string? ClientId { get; set; }
        public string? ProjectId { get; set; }
        public string? AuthUri { get; set; }
        public string? TokenUri { get; set; }
        public string? AuthProviderX509CertUri { get; set; }
        public string? ApiBaseUri { get; set; }
        public string? ResponseType { get; set; }
        public string? GrantType { get; set; }
        public string? RefreshTokenGrantType { get; set; }
        public string? AccessType { get; set; }
        public string? Prompt { get; set; }
        public string[]? Scope { get; set; } = Array.Empty<string>();
        public string? IncludeGrantedScopes { get; set; }
        public PlaylistParametersConfig? PlaylistParameters { get; set; }
    }
}