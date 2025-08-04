using System;
using Microsoft.Extensions.Options;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Contracts.Helper.YouTubeMusic;

namespace MusicTransify.src.Utilities.Playlist.YouTubeMusic
{
    public class YouTubeMusicPlaylistHelper : IYouTubeMusicPlaylistHelper
    {
        private readonly SpotifyOptions _options;
        private readonly ILogger<YouTubeMusicPlaylistHelper> _logger;
        public YouTubeMusicPlaylistHelper
        (
            IOptions<SpotifyOptions> options,
            ILogger<YouTubeMusicPlaylistHelper> logger
        )
        {
            _options = options.Value;
            _logger = logger;
        }

        public HttpRequestMessage BuildPlaylistRequest(string playlistId)
        {
            _logger.LogInformation("Building YouTube Music playlist request URL...");

            string authUri = _options.AuthUri;

            if (string.IsNullOrEmpty(playlistId))
            {
                throw new InvalidOperationException(nameof(playlistId));
            }

            if (string.IsNullOrEmpty(authUri))
            {
                throw new InvalidOperationException(nameof(authUri));
            }

            return new HttpRequestMessage(HttpMethod.Get, $"{authUri}/playlists/{playlistId}");
        }

        public HttpRequestMessage BuildPlaylistRequest()
        {
            throw new NotImplementedException();
        }

        public HttpRequestMessage BuildPlaylistRequestWithId(string playlistId)
        {
            throw new NotImplementedException();
        }

        public string ClientName => _options.ClientName;
    }
}