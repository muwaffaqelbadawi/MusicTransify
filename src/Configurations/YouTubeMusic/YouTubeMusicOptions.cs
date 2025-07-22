using System;
using System.ComponentModel.DataAnnotations;

namespace MusicTransify.src.Configurations.YouTubeMusic
{
    // POCO (Plain Old CLR Object) Entities
    // Represents the configuration options for YouTube Music API
    public class YouTubeMusicOptions
    {
        [Required]
        public string ClientName { get; set; } = string.Empty;
        [Required]
        public string ClientId { get; set; } = string.Empty;
        [Required]
        public string ProjectId { get; set; } = string.Empty;
        [Required]
        public string AuthUri { get; set; } = string.Empty;
        [Required]
        public string TokenUri { get; set; } = string.Empty;
        [Required]
        public string AuthProviderX509CertUri { get; set; } = string.Empty;
        [Required]
        public string ClientSecret { get; set; } = string.Empty;
        [Required]
        public string RedirectUri { get; set; } = string.Empty;
        [Required]
        public string ApiBaseUri { get; set; } = string.Empty;
        [Required]
        public string ResponseType { get; set; } = string.Empty;
        [Required]
        public string GrantType { get; set; } = string.Empty;
        [Required]
        public string AccessType { get; set; } = string.Empty;
        [Required]
        public string Prompt { get; set; } = string.Empty;
        [Required]
        public List<string> Scope { get; set; } = new List<string>();
    }
}