using System;
using Microsoft.Extensions.Options;

namespace MusicTransify.src.Configurations.Spotify
{
    // POCO (Plain Old CLR Object) Entities
    // Represents the configuration options for Spotify API
    public class SpotifyOptions
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string AuthUri { get; set; } = string.Empty;
        public string TokenUri { get; set; } = string.Empty;
        public string ApiBaseUri { get; set; } = string.Empty;
        public string PlaylistBaseUri { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
        public string ShowDialog { get; set; } = string.Empty;
        public string Cookie { get; set; } = string.Empty;
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }
}