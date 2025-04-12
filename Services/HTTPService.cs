using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }
}