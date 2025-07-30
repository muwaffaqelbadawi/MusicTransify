using System;
using Polly;

namespace MusicTransify.src.Contracts.RetryPolicy.YouTubeMusic
{
    public interface IYouTubeMusicRetryPolicy
    {
        IAsyncPolicy<HttpResponseMessage> RetryPolicy()
            => Policy.NoOpAsync<HttpResponseMessage>();
    }
}