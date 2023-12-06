#if NET8_0_OR_GREATER

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics;

namespace HamedStack.AspNetCore.Middlewares.GlobalException;

/// <summary>
/// Provides a global exception handling mechanism for the application.
/// </summary>
/// <remarks>
/// This class implements <see cref="IExceptionHandler"/> to provide custom error handling.
/// It logs the exception details and returns a standardized problem details response.
/// </remarks>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _env;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger used for logging error details.</param>
    /// <param name="env">The web host environment to determine the running environment.</param>
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Tries to handle exceptions globally for the application.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <param name="exception">The exception to be handled.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> representing the asynchronous operation,
    /// with a result of <see cref="bool"/> indicating whether the exception was handled.
    /// </returns>
    /// <remarks>
    /// Logs the exception details and writes a problem details response to the HTTP context.
    /// </remarks>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken = default)
    {
        _logger.LogError(exception, exception.Message);
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        ProblemDetails problemDetails = new()
        {
            Detail = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred on the server.",
            Status = StatusCodes.Status500InternalServerError,
            Title = "An internal server error has occurred.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Instance = context.Request.GetDisplayUrl()
        };
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        problemDetails.Extensions.Add("traceId", traceId);
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        return true;
    }
}
#endif