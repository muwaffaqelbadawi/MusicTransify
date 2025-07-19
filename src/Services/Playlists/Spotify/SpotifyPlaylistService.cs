using System;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Services.HTTP;

namespace MusicTransify.src.Services.Playlists.Spotify
{
    public class SpotifyPlaylistService : HttpService
    {
        private readonly SpotifyOptions _spotifyOptions;
        private readonly ILogger<SpotifyPlaylistService> _logger;

        public SpotifyPlaylistService(
            HttpClient httpClient,
            SpotifyOptions spotifyOptions,
            ILogger<SpotifyPlaylistService> logger
        ) : base(httpClient, logger)
        {
            _spotifyOptions = spotifyOptions;
            _logger = logger;
        }

        public async Task<T> GetPlaylistAsync<T>(string id)
        {
            _logger.LogInformation("Playlist service accessed");

            var clientName = _spotifyOptions.ClientName;
            string playlistUrl = _spotifyOptions.PlaylistUrl;

            if (_spotifyOptions is null)
            {
                throw new InvalidOperationException("SpotifyOptions is not configured.");
            }

            var response = new HttpRequestMessage(HttpMethod.Get, $"{playlistUrl}{id}");

            if (response is null)
            {
                throw new HttpRequestException("No response received from Spotify");
            }

            return await SendRequestAsync<T>(clientName, response);
        }
    }
}