using System;
using Polly;

namespace MusicTransify.src.Contracts.Infrastructure.ProviderRetryPolicy.Spotify
{
    public interface ISpotifyRetryPolicy
    {
        IAsyncPolicy<HttpResponseMessage> RetryPolicy()
            => Policy.NoOpAsync<HttpResponseMessage>();
    }
}