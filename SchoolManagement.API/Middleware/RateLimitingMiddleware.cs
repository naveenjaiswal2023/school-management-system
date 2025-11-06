using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace SchoolManagement.API.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private readonly int _maxRequests = 100; // Max requests per window
        private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1); // Time window

        public RateLimitingMiddleware(
            RequestDelegate next,
            IMemoryCache cache,
            ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientId = GetClientIdentifier(context);
            var cacheKey = $"rate_limit_{clientId}";

            if (_cache.TryGetValue(cacheKey, out RequestCounter counter))
            {
                if (counter.Count >= _maxRequests)
                {
                    _logger.LogWarning("Rate limit exceeded for client: {ClientId}", clientId);
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                    return;
                }

                counter.Count++;
                _cache.Set(cacheKey, counter, counter.ExpiresAt);
            }
            else
            {
                var newCounter = new RequestCounter
                {
                    Count = 1,
                    ExpiresAt = DateTime.UtcNow.Add(_timeWindow)
                };

                _cache.Set(cacheKey, newCounter, _timeWindow);
            }

            await _next(context);
        }

        private string GetClientIdentifier(HttpContext context)
        {
            // Use IP address as client identifier
            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private class RequestCounter
        {
            public int Count { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
    }
}
