using System;
using MusicTransify.src.Api.Endpoints.DTOs.Responses.Playlists.YouTubeMusic;
using MusicTransify.src.Contracts.Services.Http.YouTubeMusic;
using MusicTransify.src.Contracts.Services.Playlists.YouTubeMusic;
using MusicTransify.src.Contracts.Utilities.YouTubeMusic;
using MusicTransify.src.Utilities.DTO.YouTubeMusic;

namespace MusicTransify.src.Services.Playlists.YouTubeMusic
{
    public class YouTubeMusicPlaylistsService : IYouTubeMusicPlaylistsService
    {
        private readonly IYouTubeMusicHttpService _httpService;
        private readonly IYouTubeMusicPlaylistsHelper _playlistHelper;
        private readonly ILogger<YouTubeMusicPlaylistsService> _logger;

        public YouTubeMusicPlaylistsService(
            IYouTubeMusicHttpService httpService,
            IYouTubeMusicPlaylistsHelper playlistHelper,
            ILogger<YouTubeMusicPlaylistsService> logger
        )
        {
            _httpService = httpService;
            _playlistHelper = playlistHelper;
            _logger = logger;
        }

        public async Task<YouTubeMusicPlaylistsResponseWrapperDto> GetPlaylistsAsync()
        {
            _logger.LogInformation("Getting YouTube Music playlist");

            try
            {
                // Building playlists request
                var request = _playlistHelper.BuildPlaylistRequest();

                // Deserialize the response into the DTO
                var playlists = await _httpService.SendRequestAsync<YouTubeMusicPlaylistsResponseWrapperDto>(request);

                if (playlists is null)
                {
                    _logger.LogError("No playlists found.");
                }

                // All checks for API response DTOs should happen here
                if (playlists?.Items is null || playlists.Items.Count == 0)
                {
                    _logger.LogError("No playlists not found.");

                    return new YouTubeMusicPlaylistsResponseWrapperDto
                    {
                        Items = new List<YouTubeMusicPlaylistsResponseDto>()
                    };
                }

                foreach (var item in playlists?.Items ?? Enumerable.Empty<YouTubeMusicPlaylistsResponseDto>())
                {
                    var playlistId = item.Id;

                    var (snippet, hadMissing) = YouTubeMusicDtoFactoryHelper.FillSnippets(item.Snippet);

                    if (hadMissing)
                    {
                        _logger.LogWarning("Playlist {PlaylistId} had missing snippet fields. Defaults applied.", playlistId);
                    }
                }

                return playlists ?? new YouTubeMusicPlaylistsResponseWrapperDto();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("No playlist response received from YouTubeMusic", ex);
            }
        }
    }
}