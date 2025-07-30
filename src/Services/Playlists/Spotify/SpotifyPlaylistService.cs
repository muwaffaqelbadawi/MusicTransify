using System;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Services.Http.Spotify;
using MusicTransify.src.Contracts.Services.Playlist.Spotify;

namespace MusicTransify.src.Services.Playlists.Spotify
{
    public class SpotifyPlaylistService : ISpotifyPlaylistService
    {
        private readonly SpotifyOptions _spotifyOptions;
        private readonly SpotifyHttpService _spotifyHttpService;
        private readonly ILogger<SpotifyPlaylistService> _logger;

        public SpotifyPlaylistService(
            SpotifyOptions spotifyOptions,
            SpotifyHttpService spotifyHttpService,
            ILogger<SpotifyPlaylistService> logger
        )
        {
            _spotifyOptions = spotifyOptions;
            _spotifyHttpService = spotifyHttpService;
            _logger = logger;
        }

        public async Task<T> GetPlaylistAsync<T>(string id)
        {
            _logger.LogInformation("Getting Spotify playlist with ID: {id}", id);

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

            return await _spotifyHttpService.SendRequestAsync<T>(clientName, response);
        }
    }
}