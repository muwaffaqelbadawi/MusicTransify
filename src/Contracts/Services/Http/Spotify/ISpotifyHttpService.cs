using System;

namespace MusicTransify.src.Contracts.Services.Http.Spotify
{
    public interface ISpotifyHttpService
    {
        Task<T> SendRequestAsync<T>(
            HttpRequestMessage request
        );

        Task<T> PostFormUrlEncodedContentAsync<T>(
            string tokenUri,
            Dictionary<string, string> requestBody
        );

        public HttpRequestMessage GetRequest(
            string accessToken,
            string Uri
        );

        public HttpRequestMessage GetRequestWithId(
            string accessToken,
            string Uri,
            string Id
        );
    }
}