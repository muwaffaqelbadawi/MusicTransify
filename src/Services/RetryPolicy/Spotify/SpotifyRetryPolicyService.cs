using System;
using Polly;
using MusicTransify.src.Infrastructure.Resilience.Spotify;
using MusicTransify.src.Contracts.RetryPolicy.Spotify;

namespace MusicTransify.src.Services.RetryPolicy.Spotify
{
    public class SpotifyRetryPolicyService : ISpotifyRetryPolicy
    {
        public IAsyncPolicy<HttpResponseMessage> RetryPolicy()
            => SpotifyRetryPolicy.Default();
    }
}