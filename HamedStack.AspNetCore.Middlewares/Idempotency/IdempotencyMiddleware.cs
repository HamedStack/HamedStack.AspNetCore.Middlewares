// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HamedStack.AspNetCore.Middlewares.Idempotency;

/// <summary>
/// Middleware to ensure idempotency of requests.
/// </summary>
public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IIdempotencyCacheService? _cacheService;
    private readonly TimeSpan? _defaultExpirationTime;
    private readonly int? _rateLimit;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdempotencyMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware delegate in the pipeline.</param>
    /// <param name="idempotencySettings">Settings related to idempotency.</param>
    /// <param name="cacheService">Optional service for caching idempotent request data.</param>
    public IdempotencyMiddleware(RequestDelegate next,
                                 IOptions<IdempotencySettings> idempotencySettings,
                                 IIdempotencyCacheService? cacheService = null)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _cacheService = cacheService;

        _defaultExpirationTime = idempotencySettings.Value.DefaultExpirationTime;
        _rateLimit = idempotencySettings.Value.RateLimit;
    }

    /// <summary>
    /// Invokes the middleware, handling idempotent request logic.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
    {
        var idempotencyKey = context.Request.Headers["x-idempotency-key"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            await _next(context);
            return;
        }

        if (_rateLimit.HasValue && _cacheService != null)
        {
            var canProceed = await _cacheService.CanProceedWithRequest(idempotencyKey, _rateLimit.Value);
            if (!canProceed)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Rate Limit Exceeded",
                    Status = 429,
                    Detail = "Request limit reached for this idempotency key.",
                    Type = "https://tools.ietf.org/html/rfc6585#section-4"
                };

                context.Response.StatusCode = problemDetails.Status.Value;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                return;
            }
        }

        if (_cacheService != null)
        {
            var cachedResponse = await _cacheService.GetCacheResponseAsync(idempotencyKey);
            if (cachedResponse != null)
            {
                context.Response.StatusCode = cachedResponse.StatusCode;
                await context.Response.WriteAsync(cachedResponse.Data);
                return;
            }
        }

        await _next(context);

        if (_cacheService != null && !string.IsNullOrWhiteSpace(idempotencyKey))
        {
            var responseContent = await ReadBodyAsStringAsync(context.Response);
            await _cacheService.SetCacheResponseAsync(idempotencyKey, new IdempotencyCachedResponse
            {
                StatusCode = context.Response.StatusCode,
                Data = responseContent,
                Timestamp = DateTimeOffset.UtcNow
            }, _defaultExpirationTime);
        }
    }

    /// <summary>
    /// Reads the content of the HttpResponse body as a string.
    /// </summary>
    /// <param name="response">The HttpResponse object.</param>
    /// <returns>A string representing the response body content.</returns>
    private static async Task<string> ReadBodyAsStringAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(response.Body, Encoding.UTF8);
        var body = await reader.ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }
}