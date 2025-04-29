using System;
using System.Text.Json;
using System.Net.Http.Headers;

namespace SpotifyWebAPI_Intro.Services
{
    public class HttpService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly OptionsService _optionsService;
        private readonly SessionService _sessionService;
        private readonly ILogger<HttpService> _logger;

        public HttpService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient, OptionsService optionsService, SessionService sessionService, ILogger<HttpService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _optionsService = optionsService;
            _sessionService = sessionService;
            _logger = logger;
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

            // Getting the result as a response
            var result = await response.Content.ReadAsStringAsync();

            try
            {
                // return Token Info
                return JsonSerializer.Deserialize<JsonElement>(result);
            }
            catch (JsonException ex)
            {
                // Optionally log
                _logger.LogError(ex, "Failed to deserialize response from {Url}. Raw: {Raw}", url, result);
                throw new Exception("Failed to deserialize response from Spotify.", ex);
            }
        }
        public async Task<HttpResponseMessage> GetHttpResponseAsync(string endPoint)
        {
            // Set access token
            var accessToken = _sessionService.GetTokenInfo("access_token");

            // Set APIBase URI
            string apiBaseURL = _optionsService.SpotifyApiBaseUrl;

            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Access token is null or empty for GET {Endpoint}", endPoint);
                throw new InvalidOperationException("access_token in is null or empty");
            }

            var url = $"{apiBaseURL}{endPoint}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            _logger.LogDebug("Sending GET request to {Url}", url);

            var response = await _httpClient.SendAsync(request);
            // Do not dispose response here; caller is responsible.

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("GET {Url} failed: {StatusCode} {ErrorContent}", url, response.StatusCode, errorContent);
            }

            return response;
        }

        public void AppendCookies(string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                _logger.LogWarning("Attempted to append empty state cookie.");
                return;
            }

            _httpContextAccessor?.HttpContext?.Response.Cookies.Append(
            "spotify_auth_state",
            state,
            new CookieOptions { HttpOnly = true, Secure = true }
            );
        }
    }
}