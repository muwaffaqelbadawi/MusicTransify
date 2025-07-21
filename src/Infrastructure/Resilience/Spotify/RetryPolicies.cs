using System;
using Polly;
using Polly.Extensions.Http;

namespace MusicTransify.src.Infrastructure.Resilience.Spotify
{
    public static class RetryPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> SpotifyRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                );
        }
    }
}