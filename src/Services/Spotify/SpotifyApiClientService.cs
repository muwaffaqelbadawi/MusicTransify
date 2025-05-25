using System;
using SpotifyWebAPI_Intro.src.Configurations.Spotify;
using SpotifyWebAPI_Intro.src.Services.Common;

namespace SpotifyWebAPI_Intro.Services.Spotify
{
    public class SpotifyApiClientService : HttpService
    {
        private readonly string _serviceName = "Spotify";
        private readonly SpotifyOptionsProvider _spotifyOptionsProvider;

        public SpotifyApiClientService(HttpClient httpClient, ILogger<HttpService> logger, SpotifyOptionsProvider spotifyOptionsProvider) : base(httpClient, logger)
        {
            _spotifyOptionsProvider = spotifyOptionsProvider;
        }

        public async Task<SpotifyService> GetPlaylistAsync(string id)
        {
            string baseUri = _spotifyOptionsProvider.ApiBaseUri;
            var response = new HttpRequestMessage(HttpMethod.Get, $"{baseUri}/playlists/{id}");

            if (response is null)
            {
                _logger.LogError("No response received from Spotify for GET {BaseUrl}/playlists/{PlaylistId}", baseUri, id);
                throw new HttpRequestException("No response received from Spotify");
            }

            return await SendRequestAsync<SpotifyService>(response, _serviceName);
        }
    }
}