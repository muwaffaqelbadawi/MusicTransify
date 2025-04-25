using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace SpotifyWebAPI_Intro.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private readonly OptionsService _optionsService;
        private readonly SessionService _sessionService;

        public HttpService(HttpClient httpClient, OptionsService optionsService, SessionService sessionService)
        {
            _httpClient = httpClient;
            _optionsService = optionsService;
            _sessionService = sessionService;
        }

        public async Task<JsonElement> PostFormUrlEncodedContentAsync(string url, Dictionary<string, string> requestBody)
        {
            // Form content
            var formContent = new FormUrlEncodedContent(requestBody);

            // Post Form Url Encoded Content
            var response = await _httpClient.PostAsync(url, formContent);

            // Handling response error
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"HTTP Request failed with status code: {response.StatusCode}");
            }

            // Getting result as a response
            var result = await response.Content.ReadAsStringAsync();

            // return Token Info
            return JsonSerializer.Deserialize<JsonElement>(result);
        }
        public async Task<HttpResponseMessage> GetHttpResponseAsync(string EndPoint)
        {
            // Set access token
            string AccessToken = _sessionService.GetTokenInfo("AccessToken");

            // Set APIBase URI
            string APIBaseURL = _optionsService.SpotifyApiBaseUrl;

            // Create Autorization String
            string Authorization = $"Bearer {AccessToken}";

            // Authorization Header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Authorization);

            // Get playlists info
            var response = await _httpClient.GetAsync($"{APIBaseURL}{EndPoint}");

            return response;
        }
    }
}