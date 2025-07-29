using System;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Services.HTTP.YouTubeMusic;
using MusicTransify.src.Contracts.Services.ProviderPlaylist.YouTubeMusic;

namespace MusicTransify.src.Services.Playlists.YouTubeMusic
{
    public class YouTubeMusicPlaylistService : IYouTubeMusicPlaylistService
    {
        private readonly YouTubeMusicOptions _youTubeMusicOptions;
        private readonly YouTubeMusicHttpService _youTubeMusicHttpService;
        private readonly ILogger<YouTubeMusicPlaylistService> _logger;
        public YouTubeMusicPlaylistService(
            YouTubeMusicOptions youTubeMusicOptions,
            YouTubeMusicHttpService youTubeMusicHttpService,
            ILogger<YouTubeMusicPlaylistService> logger
        )
        {
            _youTubeMusicOptions = youTubeMusicOptions;
            _youTubeMusicHttpService = youTubeMusicHttpService;
            _logger = logger;
        }

        public async Task<T> GetPlaylistAsync<T>(string id)
        {
            _logger.LogInformation("Getting YouTube Music playlist with ID: {id}", id);

            var clientName = _youTubeMusicOptions.ClientName;
            string playlistUrl = _youTubeMusicOptions.PlaylistUrl;

            if (_youTubeMusicOptions is null)
            {
                throw new InvalidOperationException("YouTubeMusicOptions is not configured.");
            }

            var response = new HttpRequestMessage(HttpMethod.Get, $"{playlistUrl}{id}");

            if (response is null)
            {
                throw new HttpRequestException("No response received from YouTube Music");
            }

            return await _youTubeMusicHttpService.SendRequestAsync<T>(clientName, response);
        }
    }
}