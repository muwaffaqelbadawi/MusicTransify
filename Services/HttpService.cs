using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace SpotifyWebAPI_Intro.Services
{
    public class HttpService
    {
        private readonly HttpContext _context;
        private readonly HttpClient _client;
        private readonly OptionsService _optionsService;
        private readonly SessionService _sessionService;

        public HttpService(HttpContext context, HttpClient client, OptionsService optionsService, SessionService sessionService)
        {
            _client = client;
            _context = context;
            _optionsService = optionsService;
            _sessionService = sessionService;
        }

        public async Task<JsonElement> PostFormUrlEncodedContentAsync(string url, Dictionary<string, string> requestBody)
        {
            // Form content
            var formContent = new FormUrlEncodedContent(requestBody);

            // Post Form Url Encoded Content
            var response = await _client.PostAsync(url, formContent);

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

            // Create authorization String
            string Authorization = $"Bearer {AccessToken}";

            // Authorization Header
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Authorization);

            // Get playlists info
            var response = await _client.GetAsync($"{APIBaseURL}{EndPoint}");

            return response;
        }

        public void AppendCookies(string state)
        {
            _context.Response.Cookies.Append("spotify_auth_state",
            state, new CookieOptions { HttpOnly = true, Secure = true });
        }
    }
}