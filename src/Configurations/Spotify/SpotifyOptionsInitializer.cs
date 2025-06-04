using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTransify.src.Configurations.Spotify
{
    public class SpotifyOptionsInitializer
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string AuthUri { get; set; } = string.Empty;
        public string TokenUri { get; set; } = string.Empty;
        public string APIBaseUri { get; set; } = string.Empty;
        public string PlaylistBaseUri { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
        public string ShowDialog { get; set; } = string.Empty;
        public string Cookie { get; set; } = string.Empty;

        // Headers
        public Dictionary<string, string> Headers { get; set; } = new();
    }
}