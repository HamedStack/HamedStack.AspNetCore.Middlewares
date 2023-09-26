// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HamedStack.AspNetCore.Middlewares.CustomHeader;

/// <summary>
/// Middleware to validate a custom header and its value.
/// </summary>
public class CustomHeaderValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _customHeader;
    private readonly string _expectedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomHeaderValidationMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next request delegate in the middleware pipeline.</param>
    /// <param name="options">Options for custom header configuration.</param>
    public CustomHeaderValidationMiddleware(RequestDelegate next, IOptions<CustomHeaderOptions> options)
    {
        _next = next;
        _customHeader = options.Value.HeaderName;
        _expectedValue = options.Value.HeaderExpectedValue;
    }

    /// <summary>
    /// Invokes the middleware for custom header validation.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
    {
        if (string.IsNullOrWhiteSpace(_customHeader) || string.IsNullOrWhiteSpace(_expectedValue))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.Keys.Contains(_customHeader) || context.Request.Headers[_customHeader].First() != _expectedValue)
        {
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Invalid Custom Header Value",
                Detail = $"Expected header '{_customHeader}' with value '{_expectedValue}'."
            };

            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(problemDetails));
            return;
        }

        await _next(context);
    }
}