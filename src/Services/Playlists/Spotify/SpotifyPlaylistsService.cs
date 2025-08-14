using System;
using MusicTransify.src.Contracts.Services.Playlists.Spotify;
using MusicTransify.src.Contracts.Services.Http.Spotify;
using MusicTransify.src.Contracts.Utilities.Spotify;

namespace MusicTransify.src.Services.Playlists.Spotify
{
    public class SpotifyPlaylistsService : ISpotifyPlaylistsService
    {
        private readonly ISpotifyHttpService _httpService;
        private readonly ISpotifyPlaylistsHelper _playlistHelper;
        private readonly ILogger<SpotifyPlaylistsService> _logger;

        public SpotifyPlaylistsService(
            ISpotifyHttpService httpService,
            ISpotifyPlaylistsHelper playlistHelper,
            ILogger<SpotifyPlaylistsService> logger
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
    }
}