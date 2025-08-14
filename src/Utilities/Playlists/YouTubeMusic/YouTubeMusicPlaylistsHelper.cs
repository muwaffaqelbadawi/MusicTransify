using System;
using MusicTransify.src.Contracts.Utilities.YouTubeMusic;
using MusicTransify.src.Contracts.Services.Http.YouTubeMusic;
using MusicTransify.src.Utilities.Options.YouTubeMusic;
using MusicTransify.src.Utilities.Session.YouTubeMusic;

namespace MusicTransify.src.Utilities.Playlists.YouTubeMusic
{
    public class YouTubeMusicPlaylistsHelper : IYouTubeMusicPlaylistsHelper
    {
        private readonly IYouTubeMusicHttpService _httpService;
        private readonly YouTubeMusicOptionsHelper _options;
        private readonly YouTubeMusicTokenInfoHelper _tokenInfo;
        private readonly YouTubeMusicPlaylistsQueryBuilder _queryBuilder;
        private readonly ILogger<YouTubeMusicPlaylistsHelper> _logger;
        public YouTubeMusicPlaylistsHelper
        (
            IYouTubeMusicHttpService httpService,
            YouTubeMusicOptionsHelper options,
            YouTubeMusicTokenInfoHelper tokenInfo,
            YouTubeMusicPlaylistsQueryBuilder requestBuilder,
            ILogger<YouTubeMusicPlaylistsHelper> logger
        )
        {
            _httpService = httpService;
            _options = options;
            _tokenInfo = tokenInfo;
            _queryBuilder = requestBuilder;
            _logger = logger;
        }

        public HttpRequestMessage BuildPlaylistRequest()
        {
            _logger.LogInformation("Building YouTube Music playlist request request...");

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