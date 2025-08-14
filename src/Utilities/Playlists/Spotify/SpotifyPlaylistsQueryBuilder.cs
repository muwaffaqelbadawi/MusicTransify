using System;
using MusicTransify.src.Api.Endpoints.Dtos.Requests.Playlists.Spotify;
using MusicTransify.src.Utilities.Auth.Common;

namespace MusicTransify.src.Utilities.Playlists.Spotify
{
    public class SpotifyPlaylistsQueryBuilder
    {
        private readonly AuthHelper _authHelper;
        private readonly ILogger<SpotifyPlaylistsQueryBuilder> _logger;
        public SpotifyPlaylistsQueryBuilder(
            AuthHelper authHelper,
            ILogger<SpotifyPlaylistsQueryBuilder> logger
        )
        {
            _authHelper = authHelper;
            _logger = logger;
        }

        public string BuildPlaylistsQueryString()
        {
            _logger.LogInformation("Building YouTube Music Playlist request...");

            SpotifyPlaylistsRequestDto parameter = new()
            {
                
            };

            var queryParameters = parameter.ToMap();

            string queryString = _authHelper.ToQueryString(queryParameters);

            return queryString;
        }
    }
}