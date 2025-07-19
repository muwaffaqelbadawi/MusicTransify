using System;
using MusicTransify.src.Configurations.YouTubeMusic;
using MusicTransify.src.Services.HTTP;

namespace MusicTransify.src.Services.Playlists.YouTubeMusic
{
    public class YouTubeMusicPlaylistService : HttpService
    {
        private readonly YouTubeMusicOptions _youTubeMusicOptions;
        public YouTubeMusicPlaylistService(HttpClient httpClient, ILogger<HttpService> logger, YouTubeMusicOptions youTubeMusicOptions) : base(httpClient, logger)
        {
            _youTubeMusicOptions = youTubeMusicOptions;
        }

        public async Task<T> GetPlaylistAsync<T>(string id)
        {
            var clientName = _youTubeMusicOptions.ClientName;

            string baseUri = _youTubeMusicOptions.ApiBaseUri;
            var response = new HttpRequestMessage(HttpMethod.Get, $"{baseUri}/playlist/{id}");

            if (response is null)
            {
                throw new HttpRequestException("No response received from Spotify");
            }

            return await SendRequestAsync<T>(clientName, response);
        }
    }
}