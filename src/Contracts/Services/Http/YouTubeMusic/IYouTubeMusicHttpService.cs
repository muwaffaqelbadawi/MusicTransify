using System;
using System.Text.Json;

namespace MusicTransify.src.Contracts.Services.Http.YouTubeMusic
{
    public interface IYouTubeMusicHttpService
    {
        Task<T> SendRequestAsync<T>(
            string clientName,
            HttpRequestMessage request
        );
        Task<T> PostFormUrlEncodedContentAsync<T>(
           string clientName,
           string tokenUri,
           Dictionary<string, string> requestBody
       );

        Task<T> GetHttpResponseAsync<T>(
            string clientName,
            string accessToken,
            string apiBaseUri,
            string endPoint
        );
    }
}