using System;
using MusicTransify.src.Utilities.Auth.Common;
using MusicTransify.src.Utilities.Options.YouTubeMusic;
using MusicTransify.src.Api.YouTubeMusic.Playlists.Requests;

namespace MusicTransify.src.Utilities.Playlists.YouTubeMusic
{
    public class YouTubeMusicPlaylistsQueryBuilder
    {
        private readonly YouTubeMusicOptionsHelper _options;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<YouTubeMusicPlaylistsQueryBuilder> _logger;

        public YouTubeMusicPlaylistsQueryBuilder(
            YouTubeMusicOptionsHelper options,
            AuthHelper authHelper,
            ILogger<YouTubeMusicPlaylistsQueryBuilder> logger
        )
        {
            _options = options;
            _authHelper = authHelper;
            _logger = logger;
        }

        public string BuildPlaylistsQueryString()
        {
            _logger.LogInformation("Building YouTube Music Playlist request...");

            YouTubeMusicPublicPlaylistsRequestDto parameters = new()
            {
                PlaylistParameters = new YouTubeMusicPlaylistParameters
                {
                    Part = _options.PlaylistParameters.Part,
                    Mine = _options.PlaylistParameters.Mine
                }
            };

            var queryParameters = parameters.ToMap();

            string queryString = _authHelper.ToQueryString(queryParameters);

            return queryString;
        }
    }
}