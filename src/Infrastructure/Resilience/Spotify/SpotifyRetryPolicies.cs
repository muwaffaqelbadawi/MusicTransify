using System;
using Polly;
using Polly.Extensions.Http;

namespace MusicTransify.src.Infrastructure.Resilience.Spotify
{
    public static class SpotifyRetryPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> Default() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))
                );
    }


}