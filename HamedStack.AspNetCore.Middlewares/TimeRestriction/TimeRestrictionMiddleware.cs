// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HamedStack.AspNetCore.Middlewares.TimeRestriction;

/// <summary>
/// Middleware for restricting requests based on time.
/// </summary>
public class TimeRestrictionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TimeRestrictionOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeRestrictionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="options">The options for time-based request restriction.</param>
    public TimeRestrictionMiddleware(RequestDelegate next, IOptions<TimeRestrictionOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    /// <summary>
    /// Invokes the time restriction middleware.
    /// </summary>
    /// <param name="context">The HTTP context for the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
    {
        var currentTime = DateTime.Now.TimeOfDay;
        if (currentTime < _options.StartTime || currentTime > _options.EndTime)
        {
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            return;
        }

        await _next(context);
    }
}