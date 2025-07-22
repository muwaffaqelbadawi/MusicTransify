using System;
using System.ComponentModel.DataAnnotations;

namespace MusicTransify.src.Configurations.Spotify
{
    // POCO (Plain Old CLR Object) Entities
    // Represents the configuration options for Spotify API
    public class SpotifyOptions
    {
        [Required]
        public string ClientName { get; set; } = string.Empty;
        [Required]
        public string ClientId { get; set; } = string.Empty;
        [Required]
        public string ClientSecret { get; set; } = string.Empty;
        [Required]
        public string RedirectUri { get; set; } = string.Empty;
        [Required]
        public string AuthUri { get; set; } = string.Empty;
        [Required]
        public string TokenUri { get; set; } = string.Empty;
        [Required]
        public string ApiBaseUri { get; set; } = string.Empty;
        [Required]
        public string GrantType { get; set; } = string.Empty;
        [Required]
        public string ResponseType { get; set; } = string.Empty;
        [Required]
        public string ShowDialog { get; set; } = string.Empty;
        [Required]
        public string PlaylistEndpoind { get; set; } = string.Empty;
        [Required]
        public string PlaylistUrl { get; set; } = string.Empty;
        [Required]
        public List<string> Scope { get; set; } = new List<string>();
    }
}