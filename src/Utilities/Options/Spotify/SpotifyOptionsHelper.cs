using System;
using Microsoft.Extensions.Options;
using MusicTransify.src.Configurations.Spotify;

namespace MusicTransify.src.Utilities.Options.Spotify
{
    public class SpotifyOptionsHelper
    {
        private readonly SpotifyOptions _options;
        public SpotifyOptionsHelper
        (
            IOptions<SpotifyOptions> options
        )
        {
            _options = options.Value;
        }
        public string ClientName => _options.ClientName ?? string.Empty;
        public string ClientId => _options.ClientId ?? string.Empty;
        public string ClientSecret => _options.ClientSecret ?? string.Empty;
        public string RedirectUri => _options.RedirectUri ?? string.Empty;
        public string AuthUri => _options.AuthUri ?? string.Empty;
        public string TokenUri => _options.TokenUri ?? string.Empty;
        public string ApiBaseUri => _options.ApiBaseUri ?? string.Empty;
        public string GrantType => _options.GrantType ?? string.Empty;
        public string RefreshTokenGrantType => _options.RefreshTokenGrantType ?? string.Empty;
        public string ResponseType => _options.ResponseType ?? string.Empty;
        public string ShowDialog => _options.ShowDialog ?? string.Empty;
        public string[] Scope { get; set; } = Array.Empty<string>();
    }
}
