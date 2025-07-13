using System;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Services.Common;

namespace MusicTransify.src.Services.YouTubeMusic
{
    public class YouTubeMusicApiClientService : HttpService
    {
        private readonly string _serviceName = "YouTubeMusic";
        private readonly YouTubeMusicOptions _youTubeMusicOptions;
        public YouTubeMusicApiClientService(HttpClient httpClient, ILogger<HttpService> logger, YouTubeMusicOptions youTubeMusicOptions) : base(httpClient, logger)
        {
            _youTubeMusicOptions = youTubeMusicOptions;
        }

        public async Task<T> GetPlaylistAsync<T>(string id)
        {
            string baseUri = _youTubeMusicOptions.ApiBaseUri;
            var response = new HttpRequestMessage(HttpMethod.Get, $"{baseUri}/playlists/{id}");

            if (response is null)
            {
                _logger?.LogError("No response received from YouTubeMusic for GET {BaseUrl}/playlists/{PlaylistId}", baseUri, id);
                throw new HttpRequestException("No response received from Spotify");
            }

            return await SendRequestAsync<T>(response, _serviceName);
        }
    }
}