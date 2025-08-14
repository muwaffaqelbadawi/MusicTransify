using System;
using MusicTransify.src.Contracts.Utilities.Spotify;
using MusicTransify.src.Contracts.Services.Http.Spotify;
using MusicTransify.src.Utilities.Options.Spotify;
using MusicTransify.src.Utilities.Session.Spotify;

namespace MusicTransify.src.Utilities.Playlists.Spotify
{
    public class SpotifyPlaylistsHelper : ISpotifyPlaylistsHelper
    {
        private readonly ISpotifyHttpService _httpService;
        private readonly SpotifyOptionsHelper _options;
        private readonly SpotifyTokenInfoHelper _tokenInfo;
        private readonly SpotifyPlaylistsQueryBuilder _queryBuilder;
        private readonly ILogger<SpotifyPlaylistsHelper> _logger;
        public SpotifyPlaylistsHelper
        (
            SpotifyOptionsHelper options,
            ISpotifyHttpService httpService,
            SpotifyTokenInfoHelper tokenInfo,
            SpotifyPlaylistsQueryBuilder queryBuilder,
            ILogger<SpotifyPlaylistsHelper> logger
        )
        {
            _options = options;
            _httpService = httpService;
            _tokenInfo = tokenInfo;
            _queryBuilder = queryBuilder;
            _logger = logger;
        }

        public HttpRequestMessage BuildPlaylistRequest()
        {
            _logger.LogInformation("Building Spotify playlist request URL...");

            string accessToken = _tokenInfo.AccessToken;
            string apiBaseUri = _options.ApiBaseUri;
            string endpoint = "playlists";
            string queryString = _queryBuilder.BuildPlaylistsQueryString();

            var request = _httpService.GetRequest(
                accessToken: accessToken,
                apiBaseUri: apiBaseUri,
                endpoint: endpoint,
                queryString: queryString
            );

            return request;
        }
    }
}