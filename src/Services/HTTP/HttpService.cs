using System;
using System.Net;
using System.Text.Json;
using System.Net.Http.Headers;
using MusicTransify.src.Contracts.HTTP;

namespace MusicTransify.src.Services.HTTP
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpService> _logger;
        public HttpService(HttpClient httpClient, ILogger<HttpService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<T> SendRequestAsync<T>(string clientName,
        HttpRequestMessage request)
        {
            if (_httpClient is not null)
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    if (response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        if (response is null)
                        {
                            _logger?.LogError("No response received from {clientName} for Send request {request}", clientName, request);
                            throw new HttpRequestException("No response received from Spotify");
                        }

                        var content = await response.Content.ReadAsStringAsync();

                        var result = JsonSerializer.Deserialize<T>(content);

                        if (result is null)
                        {
                            _logger?.LogError("result is null for response: {content}", content);
                            throw new JsonException("Result is null.");
                        }

                        return result;
                    }
                }
            }

            throw new InvalidOperationException($"{clientName} is null");
        }

        public async Task<JsonElement> PostFormUrlEncodedContentAsync(
            string clientName,
            string url,
            Dictionary<string, string> requestBody
        )
        {
            if (_httpClient is not null)
            {
                try
                {
                    // Form content
                    var formContent = new FormUrlEncodedContent(requestBody);

                    // Post Form Url Encoded Content
                    using (var response = await _httpClient.PostAsync(url, formContent))
                    {
                        if (response is null)
                        {
                            _logger?.LogError("No response received from Spotify for POST {Url}", url);
                            throw new HttpRequestException("No response received from Spotify");
                        }

                        // Read the response
                        var content = await response.Content.ReadAsStringAsync();

                        // Handling response error
                        if (!response.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            _logger?.LogWarning("POST {Url} failed: {StatusCode} {Content}",
                            url, response.StatusCode, content);

                            throw new HttpRequestException($"HTTP Request failed with status: {response.StatusCode}");
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
                    _logger?.LogError(ex, "Unexpected error during POST to {Url}", url);
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
            if (_httpClient is not null)
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
                    var response = await _httpClient.SendAsync(request);
                    
                    if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        _logger?.LogWarning("GET {Url} failed: {StatusCode} {ErrorContent}", url, response.StatusCode, errorContent);
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Failed to send GET request to {Url}", url);
                    throw;
                }
            }

            throw new InvalidOperationException($"{clientName} is null");
        }
    }
}