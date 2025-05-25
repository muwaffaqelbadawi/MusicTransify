using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SpotifyWebAPI_Intro.src.Configurations.YouTubeMusic
{
    public class YouTubeMusicOptionsProvider
    {
        // Primary constructor
        private readonly YouTubeMusicOptionsInitializer _options;

        public YouTubeMusicOptionsProvider(IOptions<YouTubeMusicOptionsInitializer> options)
        {
            _options = options.Value;
        }

        public string ClientId => _options.ClientId;
        public string ProjectIdy => _options.ProjectId;
        public string AuthUri => _options.AuthUri;
        public string TokenUri => _options.TokenUri;
        public string AuthProviderX509CertUri => _options.AuthProviderX509CertUri;
        public string ClientSecret => _options.ClientSecret;
        public string RedirectUri => _options.RedirectUri;
        public string ApiBaseUri => _options.ApiBaseUri;
        public string PlaylistBaseUrl => _options.PlaylistBaseUri;
        public string Cookie => _options.Cookie;

        public Dictionary<string, string> Headers => _options.Headers;
    }
}