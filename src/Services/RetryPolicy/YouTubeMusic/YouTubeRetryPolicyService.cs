using System;
using Polly;
using MusicTransify.src.Infrastructure.Resilience.YouTubeMusic;
using MusicTransify.src.Contracts.Infrastructure.ProviderRetryPolicy.YouTubeMusic;

namespace MusicTransify.src.Services.RetryPolicy.YouTubeMusic
{
    public class YouTubeRetryPolicyService : IYouTubeMusicRetryPolicy
    {
        public IAsyncPolicy<HttpResponseMessage> RetryPolicy()
            => YouTubeRetryPolicy.Default();
    }
}