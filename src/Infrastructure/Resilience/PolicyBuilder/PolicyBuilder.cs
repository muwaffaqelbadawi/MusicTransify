using System;
using Polly;
using Polly.Extensions.Http;

namespace MusicTransify.src.Infrastructure.Resilience.PolicyBuilder
{
    public static class PolicyBuilder
    {
        public static IAsyncPolicy<HttpResponseMessage> CreateResiliencePolicy()
        {
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds}s due to {outcome.Exception?.Message}");
                    });

            var circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, breakDelay) =>
                    {
                        Console.WriteLine($"Circuit broken! Retry after {breakDelay.TotalSeconds}s.");
                    },
                    onReset: () => Console.WriteLine("Circuit reset."),
                    onHalfOpen: () => Console.WriteLine("Circuit in half-open state.")
                );

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);

            var fallbackPolicy = Policy<HttpResponseMessage>
                .Handle<Exception>()
                .FallbackAsync(
                    fallbackValue: new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StringContent("Fallback response")
                    },
                    onFallbackAsync: async b =>
                    {
                        Console.WriteLine("Fallback triggered due to: " + b.Exception.Message);
                        await Task.CompletedTask;
                    });

            return Policy.WrapAsync(fallbackPolicy, circuitBreakerPolicy, retryPolicy, timeoutPolicy);
        }
    }
}
