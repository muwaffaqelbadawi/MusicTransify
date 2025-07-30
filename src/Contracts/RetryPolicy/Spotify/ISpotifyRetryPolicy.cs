using System;
using Polly;

namespace MusicTransify.src.Contracts.RetryPolicy.Spotify
{
    public interface ISpotifyRetryPolicy
    {
        IAsyncPolicy<HttpResponseMessage> RetryPolicy()
            => Policy.NoOpAsync<HttpResponseMessage>();
    }
}