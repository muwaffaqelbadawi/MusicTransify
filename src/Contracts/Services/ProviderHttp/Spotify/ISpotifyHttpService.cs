using System;
using System.Text.Json;

namespace MusicTransify.src.Contracts.Services.ProviderHttp.Spotify
{
    public interface ISpotifyHttpService
    {
        Task<JsonElement> PostFormUrlEncodedContentAsync(
            string clientName,
            string tokenUri,
            Dictionary<string, string> requestBody
        );
        
        Task<HttpResponseMessage> GetHttpResponseAsync(
            string clientName,
            string accessToken,
            string apiBaseUri,
            string endPoint
        );
    }
}