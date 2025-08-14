using System;
using System.Text.Json;
using System.Net.Http.Headers;
using MusicTransify.src.Contracts.Services.Http.YouTubeMusic;
using MusicTransify.src.Utilities.Auth.Common;

namespace MusicTransify.src.Services.Http.YouTubeMusic
{
    public class YouTubeMusicHttpService : IYouTubeMusicHttpService
    {
        private readonly HttpClient _client;
        private readonly AuthHelper _authHelper;
        private readonly ILogger<YouTubeMusicHttpService> _logger;
        public YouTubeMusicHttpService(
            HttpClient client,
            AuthHelper authHelper,
            ILogger<YouTubeMusicHttpService> logger
        )
        {
            _client = client;
            _authHelper = authHelper;
            _logger = logger;
        }

        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<T> SendRequestAsync<T>(
            HttpRequestMessage request
        )
        {
            _logger?.LogInformation(
                "Sending HTTP request to YouTube with {method} {url}",
                request.Method,
                request.RequestUri
            );

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();

                _logger?.LogError(
                    "HTTP request to YouTube failed with status code {statusCode} and message: {error}",
                    response.StatusCode,
                    error
                );

                throw new HttpRequestException($"Request failed: {response.StatusCode}");
            }

            string content = await response.Content.ReadAsStringAsync();

            try
            {
                var result = JsonSerializer.Deserialize<T>(content, _serializerOptions);

                if (result is null)
                {
                    _logger?.LogError(
                        "Failed to deserialize response from YouTube. Content: {content}",
                        content
                    );

                    throw new JsonException("Deserialized result is null.");
                }

                return result;
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex,
                    "JSON deserialization failed for YouTube. Content: {content}",
                    content
                );

                throw new JsonException("Failed to deserialize response from YouTube", ex);
            }
            finally
            {
                request.Dispose();
            }
        }

        public async Task<T> PostFormUrlEncodedContentAsync<T>(
            string tokenUri,
            Dictionary<string, string> requestBody
        )
        {
            if (_client is not null)
            {
                try
                {
                    var formContent = new FormUrlEncodedContent(requestBody);

                    using (var response = await _client.PostAsync(tokenUri, formContent))
                    {
                        if (response is null)
                        {
                            _logger?.LogError(
                                "No response received from Spotify for POST {tokenUri}",
                                tokenUri
                            );

                            throw new HttpRequestException("No response received from YouTube");
                        }

                        var content = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            _logger?.LogWarning(
                                "POST {tokenUri} failed: {StatusCode} {Content}",
                                tokenUri,
                                response.StatusCode,
                                content
                            );

                            throw new HttpRequestException(
                                $"HTTP Request failed with status: {response.StatusCode}"
                            );
                        }

                        try
                        {
                            return JsonSerializer.Deserialize<T>(content) ??
                                throw new JsonException(nameof(content));
                        }

                        catch (JsonException ex)
                        {
                            _logger?.LogError(ex,
                                "Failed to deserialize response: {Content}",
                                content
                            );

                            throw new JsonException(
                                $"Failed to parse YouTube response, {nameof(content)}",
                                ex
                            );
                        }
                    }
                }
                catch (Exception ex) when (ex is not HttpRequestException && ex is not JsonException)
                {
                    _logger?.LogError(
                        ex,
                        "Unexpected error during POST to {tokenUri}",
                        tokenUri
                    );

                    throw new InvalidOperationException(
                        $"Unexpected error during POST to {nameof(tokenUri)}"
                    );
                }
            }

            throw new InvalidOperationException(
                $"Unexpected error during POST to {nameof(tokenUri)}"
            );
        }

        // This method creates a GET request message to the YouTube API for different endpoints.
        // Structure: https://{apiBaseUri}/{endpoint}?{queryString}
        // Structure: https://www.googleapis.com/youtube/v3/{endpoint}?{queryString}
        public HttpRequestMessage GetRequest(
            string accessToken,
            string apiBaseUri,
            string endpoint,
            string queryString
        )
        {
            try
            {
                _logger.LogInformation("Make a GET request message to the YouTube API");

                var request = new HttpRequestMessage(HttpMethod.Get,
                    _authHelper.BuildApiUri(apiBaseUri, endpoint, queryString)
                );

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                return request;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(
                    ex, "Failed to create GET request for YouTube API");

                throw new HttpRequestException(
                    $"{ex} Failed to create GET request for YouTube API"
                );
            }
        }
    }
}