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
            try
            {
                // Form content
                var formContent = new FormUrlEncodedContent(requestBody);

                // Post Form Url Encoded Content
                var response = await _httpClient.PostAsync(url, formContent);

                if (response is null)
                {
                    _logger.LogError("No response received from Spotify for POST {Url}", url);
                    throw new HttpRequestException("No response received from Spotify");
                }

                // Read the response
                var responseContent = await response.Content.ReadAsStringAsync();

                // Handling response error
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("POST {Url} failed: {StatusCode} {Content}",
                    url, response.StatusCode, responseContent);

                    throw new HttpRequestException($"HTTP Request failed with status: {response.StatusCode}");
                }

                try
                {
                    // return Token Info
                    return JsonSerializer.Deserialize<JsonElement>(responseContent);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to deserialize response: {Content}", responseContent);
                    throw new JsonException("Failed to parse Spotify response", ex);
                }
            }
            catch (Exception ex) when (ex is not HttpRequestException && ex is not JsonException)
            {
                _logger.LogError(ex, "Unexpected error during POST to {Url}", url);
                throw;
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

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GET {Url} failed: {StatusCode} {ErrorContent}", url, response.StatusCode, errorContent);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send GET request to {Url}", url);
                throw;
            }
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