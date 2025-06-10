using System;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Services.Common;

namespace MusicTransify.src.Services.YouTubeMusic
{
    public class YouTubeMusicApiClientService : HttpService
    {
        private readonly string _serviceName = "YouTubeMusic";
        private readonly SpotifyOptions _spotifyOptions;
        public YouTubeMusicApiClientService(HttpClient httpClient, ILogger<HttpService> logger, spotifyOptions spotifyOptions) : base(httpClient, logger)
        {
            _spotifyOptions = spotifyOptions;
        }

        public async Task<YouTubeMusicPlaylistService> GetPlaylistAsync(string id)
        {
            string baseUri = _spotifyOptions.ApiBaseUri;
            var response = new HttpRequestMessage(HttpMethod.Get, $"{baseUri}/playlists/{id}");

            if (response is null)
            {
                _logger?.LogError("No response received from YouTubeMusic for GET {BaseUrl}/playlists/{PlaylistId}", baseUri, id);
                throw new HttpRequestException("No response received from Spotify");
            }

            return await SendRequestAsync<YouTubeMusicPlaylistService>(response, _serviceName);
        }
    }
}