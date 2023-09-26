// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HamedStack.AspNetCore.Middlewares.RequestTiming;

/// <summary>
/// Middleware to measure the processing time of HTTP requests.
/// </summary>
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestTimingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
    /// <param name="logger">The logger instance used to log request details and processing times.</param>
    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to measure and log the processing time of the request.
    /// </summary>
    /// <param name="context">The HttpContext for the current request and response.</param>
    /// <returns>A Task representing the completion of request processing.</returns>
    public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        await _next(context);
        watch.Stop();
        var processingTime = watch.ElapsedMilliseconds;

        context.Response.Headers.Add("x-processing-time", processingTime.ToString());

        _logger.LogInformation(
            "Request {RequestMethod} {RequestPath} responded {StatusCode} in {ProcessingTime}ms. User-Agent: {UserAgent}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            processingTime,
            context.Request.Headers["User-Agent"].ToString()
        );
    }
}
