using System;

namespace MusicTransify.src.Configurations.Spotify
{
    // POCO (Plain Old CLR Object) Entities
    // Represents the configuration options for Spotify API
    public class SpotifyOptions
    {
        public string ClientName { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string AuthUri { get; set; } = string.Empty;
        public string TokenUri { get; set; } = string.Empty;
        public string ApiBaseUri { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;
        public string RefreshTokenGrantType { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
        public string ShowDialog { get; set; } = string.Empty;
        public string PlaylistEndpoind { get; set; } = string.Empty;
        public string PlaylistUrl { get; set; } = string.Empty;
        public string[] Scope { get; set; } = Array.Empty<string>();
    }
}