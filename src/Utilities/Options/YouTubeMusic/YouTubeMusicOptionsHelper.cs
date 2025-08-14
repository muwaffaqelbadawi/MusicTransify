using System;
using Microsoft.Extensions.Options;
using MusicTransify.src.Configurations.YouTubeMusic;

namespace MusicTransify.src.Utilities.Options.YouTubeMusic
{
    public class YouTubeMusicOptionsHelper
    {
        private readonly YouTubeMusicOptions _options;
        private readonly ILogger<YouTubeMusicOptionsHelper> _logger;
        public YouTubeMusicOptionsHelper(
            IOptions<YouTubeMusicOptions> options,
            ILogger<YouTubeMusicOptionsHelper> logger
        )
        {
            _options = options.Value;
            _logger = logger;
        }

        public string ClientName => _options.ClientName ?? string.Empty;
        public string ClientSecret => _options.ClientSecret ?? string.Empty;
        public string RedirectUri => _options.RedirectUri ?? string.Empty;
        public string ClientId => _options.ClientId ?? string.Empty;
        public string ProjectId => _options.ProjectId ?? string.Empty;
        public string AuthUri => _options.AuthUri ?? string.Empty;
        public string TokenUri => _options.TokenUri ?? string.Empty;
        public string AuthProviderX509CertUri => _options.AuthProviderX509CertUri ?? string.Empty;
        public string ApiBaseUri => _options.ApiBaseUri ?? string.Empty;
        public string ResponseType => _options.ResponseType ?? string.Empty;
        public string GrantType => _options.GrantType ?? string.Empty;
        public string RefreshTokenGrantType => _options.RefreshTokenGrantType ?? string.Empty;
        public string AccessType => _options.AccessType ?? string.Empty;
        public string Prompt => _options.Prompt ?? string.Empty;
        public string[] Scope => _options.Scope ?? Array.Empty<string>();
        public string IncludeGrantedScopes => _options.IncludeGrantedScopes ?? string.Empty;

        public PlaylistParametersConfig PlaylistParameters => new PlaylistParametersConfig
        {
            Part = _options.PlaylistParameters?.Part ?? string.Empty,
            Mine = _options.PlaylistParameters?.Mine ?? string.Empty
        };
    }
}