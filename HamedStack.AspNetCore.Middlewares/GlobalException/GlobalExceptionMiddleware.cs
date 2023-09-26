// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HamedStack.AspNetCore.Middlewares.GlobalException;

/// <summary>
/// Middleware to handle global exceptions and return standardized error responses.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger for logging exceptions.</param>
    /// <param name="env">The web hosting environment to determine runtime context.</param>
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Processes an individual HTTP request and handles global exceptions.
    /// </summary>
    /// <param name="httpContext">The context for the current request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            httpContext.Response.ContentType = "application/problem+json";
            const int statusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.StatusCode = statusCode;
            ProblemDetails problemDetails = new()
            {
                Detail = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred on the server.",
                Status = statusCode,
                Title = "An internal server error has occurred.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Instance = httpContext.Request.GetDisplayUrl()
            };
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
            problemDetails.Extensions.Add("traceId", traceId);
            var result = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(result);
        }
    }
}