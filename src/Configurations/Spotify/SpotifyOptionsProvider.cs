using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SpotifyWebAPI_Intro.src.Configurations.Spotify
{
    public class SpotifyOptionsProvider
    {
        private readonly SpotifyOptionsInitializer _options;

        public SpotifyOptionsProvider(IOptions<SpotifyOptionsInitializer> options)
        {
            _options = options.Value;
        }

        // Options properties
        public string ClientId => _options.ClientId;
        public string ClientSecret => _options.ClientSecret;
        public string RedirectUri => _options.RedirectUri;
        public string AuthUri => _options.AuthUri;
        public string TokenUri => _options.TokenUri;
        public string ApiBaseUri => _options.APIBaseUri;
        public string PlaylistBaseUri => _options.PlaylistBaseUri;
        public string Scope => _options.Scope;
        public string GrantType => _options.GrantType;
        public string ResponseType => _options.ResponseType;
        public string ShowDialog => _options.ShowDialog;
        public string Cookie => _options.Cookie;

        // Headers
        public Dictionary<string, string> Headers => _options.Headers;
    }
}