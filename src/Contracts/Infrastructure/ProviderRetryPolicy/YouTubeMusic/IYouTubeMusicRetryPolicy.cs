using System;
using Polly;

namespace MusicTransify.src.Contracts.Infrastructure.ProviderRetryPolicy.YouTubeMusic
{
    public interface IYouTubeMusicRetryPolicy
    {
        IAsyncPolicy<HttpResponseMessage> RetryPolicy()
            => Policy.NoOpAsync<HttpResponseMessage>();
    }
}