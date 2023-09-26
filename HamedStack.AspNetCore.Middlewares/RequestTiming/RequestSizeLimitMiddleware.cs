// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HamedStack.AspNetCore.Middlewares.RequestTiming;

/// <summary>
/// Middleware for limiting the size of incoming requests.
/// </summary>
public class RequestSizeLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RequestSizeLimitOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestSizeLimitMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="options">The options for request size limiting.</param>
    public RequestSizeLimitMiddleware(RequestDelegate next, IOptions<RequestSizeLimitOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    /// <summary>
    /// Invokes the request size limiting middleware.
    /// </summary>
    /// <param name="context">The HTTP context for the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
    {
        context.Request.EnableBuffering();

        if (context.Request.ContentLength > _options.MaxBytes)
        {
            context.Response.StatusCode = (int)HttpStatusCode.RequestEntityTooLarge;
            await context.Response.WriteAsync($"Request size exceeds {_options.MaxBytes} bytes limit.");
            return;
        }

        await _next(context);
    }
}