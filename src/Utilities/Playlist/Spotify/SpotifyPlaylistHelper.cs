using System;
using Microsoft.Extensions.Options;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Contracts.Helper.Spotify;
using MusicTransify.src.Contracts.Services.Http.Spotify;
using MusicTransify.src.Contracts.Session.Spotify;

namespace MusicTransify.src.Utilities.Playlist.Spotify
{
    public class SpotifyPlaylistHelper : ISpotifyPlaylistHelper
    {
        private readonly SpotifyOptions _options;
        private readonly ISpotifyHttpService _httpService;
        private readonly ISpotifySessionService _sessionService;
        private readonly ILogger<SpotifyPlaylistHelper> _logger;
        public SpotifyPlaylistHelper
        (
            IOptions<SpotifyOptions> options,
            ISpotifyHttpService httpService,
            ISpotifySessionService sessionService,
            ILogger<SpotifyPlaylistHelper> logger
        )
        {
            _options = options.Value;
            _httpService = httpService;
            _sessionService = sessionService;
            _logger = logger;
        }

        public HttpRequestMessage BuildPlaylistRequest()
        {
            {
                _logger.LogInformation("Building Spotify playlist request URL...");

                string accessToken = _sessionService.GetTokenInfo("access_token") ?? string.Empty;
                string playlistUri = _options.PlaylistUri;

                var request = _httpService.GetRequest(
                    accessToken: accessToken,
                    Uri: playlistUri
                );

                _logger.LogInformation(
                    "Access token: {accessToken}",
                    accessToken
                );

                return request;
            }
        }

        public HttpRequestMessage BuildPlaylistRequestWithId(
            string Id
        )
        {
            _logger.LogInformation("Building Spotify playlist request URL...");

            string accessToken = _sessionService.GetTokenInfo("access_token") ?? string.Empty;
            string playlistUri = _options.PlaylistUri;

            var request = _httpService.GetRequestWithId(
                accessToken: accessToken,
                Uri: playlistUri,
                Id: Id
            );

            _logger.LogInformation(
                "Access token: {accessToken}",
                accessToken
            );

            return request;
        }

        public string ClientName => _options.ClientName;
    }
}