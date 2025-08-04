using System;
using System.Net;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using MusicTransify.src.Contracts.Services.Http.Spotify;
using MusicTransify.src.Configurations.Spotify;

namespace MusicTransify.src.Services.Http.Spotify
{
    public class SpotifyHttpService : ISpotifyHttpService
    {
        private readonly HttpClient _client;
        private readonly SpotifyOptions _options;
        private readonly ILogger<SpotifyHttpService> _logger;
        public SpotifyHttpService(
            HttpClient client,
            IOptions<SpotifyOptions> options,
            ILogger<SpotifyHttpService> logger
        )
        {
            _client = client;
            _options = options.Value;
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
            string clientName = _options.ClientName;

            _logger?.LogInformation(
                "Sending HTTP request to {clientName} with {method} {url}",
                clientName,
                request.Method,
                request.RequestUri
            );

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                _logger?.LogError(
                    "HTTP request to {clientName} failed with status code {statusCode} and message: {error}",
                    clientName,
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
                        "Failed to deserialize response from {clientName}. Content: {content}",
                        clientName,
                        content
                    );
                    throw new JsonException("Deserialized result is null.");
                }

                return result;
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex,
                "JSON deserialization failed for {clientName}. Content: {content}",
                clientName,
                content
                );
                throw new JsonException("Failed to deserialize response from Spotify.", ex);
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
                    // Form content
                    var formContent = new FormUrlEncodedContent(requestBody);

                    // Post Form Url Encoded Content
                    using (var response = await _client.PostAsync(tokenUri, formContent))
                    {
                        if (response is null)
                        {
                            _logger?.LogError("No response received from Spotify for POST {tokenUri}", tokenUri);
                            throw new HttpRequestException("No response received from Spotify");
                        }

                        // Read the response
                        var content = await response.Content.ReadAsStringAsync();

                        // Handling response error
                        if (!response.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            _logger?.LogWarning("POST {TokenUri} failed: {StatusCode} {Content}",
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
                            // return Token Info
                            return JsonSerializer.Deserialize<T>(content) ??
                                throw new JsonException(nameof(content));
                        }

                        catch (JsonException ex)
                        {
                            _logger?.LogError(ex, "Failed to deserialize response: {Content}", content);
                            throw new JsonException($"Failed to parse Spotify response, {nameof(content)}", ex);
                        }
                    }
                }

                catch (Exception ex) when (ex is not HttpRequestException && ex is not JsonException)
                {
                    _logger?.LogError(ex, "Unexpected error during POST to {tokenUri}", tokenUri);
                    throw;
                }
            }

            throw new InvalidOperationException(nameof(tokenUri));
        }

        public HttpRequestMessage GetRequest(
            string accessToken,
            string Uri
        )
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Uri}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return request;
        }

        public HttpRequestMessage GetRequestWithId(
            string accessToken,
            string Uri,
            string Id
        )
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Uri}/{Id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return request;
        }
    }
}