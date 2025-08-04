using System;
using MusicTransify.src.Contracts.Services.Playlist.Spotify;
using MusicTransify.src.Contracts.Services.Http.Spotify;
using MusicTransify.src.Contracts.Helper.Spotify;

namespace MusicTransify.src.Services.Playlists.Spotify
{
    public class SpotifyPlaylistService : ISpotifyPlaylistService
    {
        private readonly ISpotifyHttpService _httpService;
        private readonly ISpotifyPlaylistHelper _playlistHelper;
        private readonly ILogger<SpotifyPlaylistService> _logger;

        public SpotifyPlaylistService(
            ISpotifyHttpService httpService,
            ISpotifyPlaylistHelper playlistHelper,
            ILogger<SpotifyPlaylistService> logger
        )
        {
            _httpService = httpService;
            _playlistHelper = playlistHelper;
            _logger = logger;
        }

        public async Task<T> GetPlaylistAsync<T>()
        {
            _logger.LogInformation("Getting Spotify playlist");

            try
            {
                var request = _playlistHelper.BuildPlaylistRequest();
                return await _httpService.SendRequestAsync<T>(request);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("No playlist response received from Spotify", ex);
            }
        }

        public async Task<T> GetPlaylistWithIdAsync<T>(string id)
        {
            _logger.LogInformation("Getting Spotify playlist with a specific ID: {id}", id);

            try
            {
                var request = _playlistHelper.BuildPlaylistRequestWithId(id);
                return await _httpService.SendRequestAsync<T>(request);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("No playlist response received from Spotify", ex);
            }
        }
    }
}