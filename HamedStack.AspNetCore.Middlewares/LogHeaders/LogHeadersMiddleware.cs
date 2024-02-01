using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HamedStack.AspNetCore.Middlewares.LogHeaders;

/// <summary>
/// Middleware component for logging HTTP request headers.
/// This class provides functionality to log each header of incoming HTTP requests.
/// </summary>
public class LogHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogHeadersMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogHeadersMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware component in the pipeline.</param>
    /// <param name="logger">The logger used for logging information.</param>
    public LogHeadersMiddleware(RequestDelegate next, ILogger<LogHeadersMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Processes an individual request.
    /// </summary>
    /// <param name="context">The context for the current HTTP request.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        foreach (var header in context.Request.Headers)
        {
            _logger.LogInformation("Header: {Key}: {Value}", header.Key, header.Value);
        }

        await _next(context);
    }
}
