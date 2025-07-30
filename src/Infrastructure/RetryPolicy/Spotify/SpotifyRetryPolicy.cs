using System;
using System.Net;
using Polly;
using Polly.Extensions.Http;

namespace MusicTransify.src.Infrastructure.RetryPolicy.Spotify
{
    public static class SpotifyRetryPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> Default() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => response.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(
                    retryCount: 4,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(1.5 * attempt)
                );
    }


}