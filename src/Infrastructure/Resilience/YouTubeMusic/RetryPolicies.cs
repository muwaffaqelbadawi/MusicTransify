using System;
using Polly;
using Polly.Extensions.Http;

namespace MusicTransify.src.Infrastructure.Resilience.YouTubeMusic
{
    public class RetryPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> YouTubeMusicRetryPolicy()
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