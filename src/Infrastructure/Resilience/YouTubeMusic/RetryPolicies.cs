using System;
using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace MusicTransify.src.Infrastructure.Resilience.YouTubeMusic
{
    public static class YouTubeRetryPolicy
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