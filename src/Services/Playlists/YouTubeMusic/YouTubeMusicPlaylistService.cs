using System;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Contracts.Services.Playlist.YouTubeMusic;
using MusicTransify.src.Contracts.Services.Http.YouTubeMusic;
using MusicTransify.src.Contracts.Helper.YouTubeMusic;

namespace MusicTransify.src.Services.Playlists.YouTubeMusic
{
    public class YouTubeMusicPlaylistService : IYouTubeMusicPlaylistService
    {
        private readonly IYouTubeMusicHttpService _youTubeMusicHttpService;
        private readonly IYouTubeMusicPlaylistHelper _youTubeMusicPlaylistHelper;
        private readonly ILogger<YouTubeMusicPlaylistService> _logger;
        public YouTubeMusicPlaylistService(
            IYouTubeMusicHttpService youTubeMusicHttpService,
            IYouTubeMusicPlaylistHelper youTubeMusicPlaylistHelper,
            ILogger<YouTubeMusicPlaylistService> logger
        )
        {
            _youTubeMusicHttpService = youTubeMusicHttpService;
            _youTubeMusicPlaylistHelper = youTubeMusicPlaylistHelper;
            _logger = logger;
        }

        public async Task<T> GetPlaylistAsync<T>()
        {
            _logger.LogInformation("Getting YouTube Music playlist");

            var clientName = _youTubeMusicPlaylistHelper.ClientName;

            if (string.IsNullOrEmpty(clientName))
            {
                throw new InvalidOperationException(nameof(clientName));
            }

            try
            {
                using (var response = _youTubeMusicPlaylistHelper.BuildPlaylistRequest())
                {
                    return await _youTubeMusicHttpService.SendRequestAsync<T>(clientName, response);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("No playlist response received from YouTube Music", ex);
            }
        }

        public Task<T> GetPlaylistAsync<T>(string id)
        {
            throw new NotImplementedException();
        }
    }
}