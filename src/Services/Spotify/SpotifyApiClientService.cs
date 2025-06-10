using System;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Services.Common;

namespace MusicTransify.src.Services.Spotify
{
    public class SpotifyApiClientService : HttpService
    {
        private readonly string _serviceName = "Spotify";
        private readonly SpotifyOptions? _spotifyOptions;

        public SpotifyApiClientService(HttpClient httpClient, ILogger<HttpService> logger, SpotifyOptions spotifyOptions) : base(httpClient, logger)
        {
            _spotifyOptions = spotifyOptions;
        }

        public async Task<T> GetPlaylistAsync<T>(string id)
        {
            if (_spotifyOptions is null)
            {
                throw new InvalidOperationException("SpotifyOptions is not configured.");
            }

            string baseUri = _spotifyOptions.ApiBaseUri;
            var response = new HttpRequestMessage(HttpMethod.Get, $"{baseUri}/playlists/{id}");

            if (response is null)
            {
                _logger.LogError("No response received from Spotify for GET {BaseUrl}/playlists/{PlaylistId}", baseUri, id);
                throw new HttpRequestException("No response received from Spotify");
            }

            return await SendRequestAsync<T>(response, _serviceName);
        }
    }
}