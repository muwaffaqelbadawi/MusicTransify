using System;
using Microsoft.Extensions.Options;

namespace MusicTransify.src.Configurations.YouTubeMusic
{
    public class YouTubeMusicOptions
    {
        private readonly YouTubeMusicOption _options;

        public YouTubeMusicOptions(IOptions<YouTubeMusicOption> options)
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
        public string ResponseType => _options.ResponseType;
        public string Scope => _options.Scope;
        public string GrantType => _options.GrantType;
        public string AccessType => _options.AccessType;
        public string Prompt => _options.Prompt;
        public string Cookie => _options.Cookie;

        public Dictionary<string, string> Headers => _options.Headers;
    }
}