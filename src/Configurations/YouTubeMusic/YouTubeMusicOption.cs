using System;

namespace MusicTransify.src.Configurations.YouTubeMusic
{
    public class YouTubeMusicOption
    {
        public string ClientId { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public string AuthUri { get; set; } = string.Empty;
        public string TokenUri { get; set; } = string.Empty;
        public string AuthProviderX509CertUri { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string ApiBaseUri { get; set; } = string.Empty;
        public string PlaylistBaseUri { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
        public string Cookie { get; set; } = string.Empty;

        public Dictionary<string, string> Headers { get; set; } = new();
    }
}