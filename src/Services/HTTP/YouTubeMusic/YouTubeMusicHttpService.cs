using System;
using System.Net;
using System.Text.Json;
using System.Net.Http.Headers;
using MusicTransify.src.Contracts.Services.Http.YouTubeMusic;

namespace MusicTransify.src.Services.Http.YouTubeMusic
{
    public class YouTubeMusicHttpService : IYouTubeMusicHttpService
    {
        private readonly HttpClient _client;
        private readonly ILogger<YouTubeMusicHttpService> _logger;
        public YouTubeMusicHttpService(
            HttpClient client,
            ILogger<YouTubeMusicHttpService> logger
        )
        {
            _client = client;
            _logger = logger;
        }

        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<T> SendRequestAsync<T>(
            string clientName,
            HttpRequestMessage request
        )
        {
            _logger?.LogInformation("Sending HTTP request to {clientName} with {method} {url}", clientName, request.Method, request.RequestUri);

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger?.LogError("HTTP request to {clientName} failed with status code {statusCode} and message: {error}", clientName, response.StatusCode, error);
                throw new HttpRequestException($"Request failed: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            #if DEBUG
            _logger?.LogDebug("Received response from {clientName}: {content}", clientName, content);
            #endif

            try
            {
                var result = JsonSerializer.Deserialize<T>(content, _serializerOptions);

                if (result is null)
                {
                    _logger?.LogError("Failed to deserialize response from {clientName}. Content: {content}", clientName, content);
                    throw new JsonException("Deserialized result is null.");
                }

                return result;
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex, "JSON deserialization failed for {clientName}. Content: {content}", clientName, content);
                throw;
            }
        }

        public async Task<JsonElement> PostFormUrlEncodedContentAsync(
            string clientName,
            string tokenUri,
            Dictionary<string, string> requestBody
        )
        {
            if (_client is not null)
            {
                try
                {
                    // Form content
                    var formContent = new FormUrlEncodedContent(requestBody);

                    // Post Form Url Encoded Content
                    using (var response = await _client.PostAsync(tokenUri, formContent))
                    {
                        if (response is null)
                        {
                            _logger?.LogError("No response received from Spotify for POST {TokenUri}", tokenUri);
                            throw new HttpRequestException("No response received from Spotify");
                        }

                        // Read the response
                        var content = await response.Content.ReadAsStringAsync();

                        // Handling response error
                        if (!response.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            _logger?.LogWarning("POST {TokenUri} failed: {StatusCode} {Content}",
                            tokenUri, response.StatusCode, content);

                            throw new HttpRequestException(
                                $"HTTP Request failed with status: {response.StatusCode}"
                            );
                        }

                        try
                        {
                            // return Token Info
                            return JsonSerializer.Deserialize<JsonElement>(content);
                        }

                        catch (JsonException ex)
                        {
                            _logger?.LogError(ex, "Failed to deserialize response: {Content}", content);
                            throw new JsonException("Failed to parse Spotify response", ex);
                        }
                    }
                }

                catch (Exception ex) when (ex is not HttpRequestException && ex is not JsonException)
                {
                    _logger?.LogError(ex, "Unexpected error during POST to {TokenUri}", tokenUri);
                    throw;
                }
            }

            throw new InvalidOperationException($"{clientName} is null");
        }

        public async Task<HttpResponseMessage> GetHttpResponseAsync(
            string clientName,
            string accessToken,
            string apiBaseUri,
            string endPoint
        )
        {
            if (_client is not null)
            {
                if (string.IsNullOrEmpty(accessToken))
                {
                    _logger?.LogWarning("Access token is null or empty for GET {Endpoint}", endPoint);
                    throw new InvalidOperationException("Access token in is null or empty");
                }

                if (string.IsNullOrEmpty(apiBaseUri))
                {
                    _logger?.LogWarning("apiBaseUri is null or empty");
                    throw new InvalidOperationException("apiBaseUri in is null or empty");
                }

                if (string.IsNullOrEmpty(endPoint))
                {
                    _logger?.LogWarning("endPoint is null or empty");
                    throw new InvalidOperationException("endPoint in is null or empty");
                }

                var url = $"{apiBaseUri}{endPoint}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Set the Authorization header with the access token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    // Remove the using block - let the caller dispose the response
                    var response = await _client.SendAsync(request);

                    if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        _logger?.LogWarning("GET {Url} failed: {StatusCode} {ErrorContent}", url, response.StatusCode, errorContent);
                    }

                    return response;
                }
                catch (JsonException ex)
                {
                    _logger?.LogError(ex, "Failed to send GET request to {Url}", url);
                    throw;
                }
            }

            throw new InvalidOperationException($"{clientName} is null");
        }
    }
}