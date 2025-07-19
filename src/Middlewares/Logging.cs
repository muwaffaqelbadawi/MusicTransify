using System;
using System.Text.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace MusicTransify.src.Middlewares
{
    public class Logging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Logging> _logger;
        public Logging(
            RequestDelegate next,
            ILogger<Logging> logger
        )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            var authStatus = context.User.Identity?.IsAuthenticated == true
                ? "Authenticated"
                : "Anonymous";

            try
            {
                _logger.LogInformation(
                    "Request: {Method} {Path} (Agent: {Agent}) IP: {IP} Request size: {Bytes} bytes Response size: {Bytes} bytes Cache: {CacheControl}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.Headers.UserAgent.ToString(),
                    context.Connection.RemoteIpAddress?.ToString(),
                    context.Request.ContentLength ?? 0,
                    context.Response.ContentLength ?? 0,
                    context.Response.Headers.CacheControl);

                // Log response with full context
                _logger.LogInformation("Response: {Method} {Path} => {StatusCode}  in {Elapsed}ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    sw.ElapsedMilliseconds);

                context.Items["CorrelationId"] = Guid.NewGuid();
                _logger.LogInformation("Request {CorrelationId}: {Method} {Path}",
                    context.Items["CorrelationId"],
                    context.Request.Method,
                    context.Request.Path);

                if (authStatus == "Authenticated")
                {
                    _logger.LogInformation("User: {UserId}",
                        context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown-id");
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                // Only handle errors if response hasn't started
                if (!context.Response.HasStarted &&
                    !context.Response.Headers.IsReadOnly)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(
                            new {
                                error = "server_error",
                                message = "Internal server error",
                                requestId = context.TraceIdentifier
                            }));
                }
                else
                {
                    _logger.LogWarning("Could not write error response - headers already sent");
                }
            }
        }
    }
}