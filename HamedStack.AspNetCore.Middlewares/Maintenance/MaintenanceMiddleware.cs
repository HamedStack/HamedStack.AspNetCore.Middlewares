// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HamedStack.AspNetCore.Middlewares.Maintenance;

/// <summary>
/// Middleware for handling maintenance mode in an ASP.NET Core application.
/// </summary>
public class MaintenanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly MaintenanceWindow _window;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaintenanceMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="window">The maintenance window configuration.</param>
    /// <param name="logger">The logger instance to use.</param>
    public MaintenanceMiddleware(RequestDelegate next, IOptions<MaintenanceWindow> window, ILogger<MaintenanceMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _window = window.Value;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
    {
        if (_window.Enabled)
        {
            _logger.LogInformation("Maintenance mode is enabled. Returning 503 Service Unavailable.");

            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;

            if (_window.RetryAfterInSeconds.HasValue)
                context.Response.Headers.Add("Retry-After", _window.RetryAfterInSeconds.ToString());

            context.Response.ContentType = _window.ContentType;
            await context
                .Response
                .WriteAsync(Encoding.UTF8.GetString(_window.Response), Encoding.UTF8);
        }
        await _next.Invoke(context);
    }
}