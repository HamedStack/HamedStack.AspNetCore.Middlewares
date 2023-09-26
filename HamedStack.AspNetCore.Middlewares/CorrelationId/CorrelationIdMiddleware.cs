// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HamedStack.AspNetCore.Middlewares.CorrelationId;

/// <summary>
/// Middleware to handle correlation ID for incoming HTTP requests.
/// </summary>
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CorrelationIdOption _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationIdMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="options">Configuration options for the correlation ID.</param>
    public CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdOption> options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _next = next ?? throw new ArgumentNullException(nameof(next));

        _options = options.Value;
    }

    /// <summary>
    /// Processes an individual HTTP request.
    /// </summary>
    /// <param name="context">The context for the current request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(_options.Key, out var correlationId))
        {
            if (correlationId.ToString() == null || string.IsNullOrWhiteSpace(correlationId.ToString()))
            {
                throw new InvalidOperationException("The correlationId extracted from the headers is null or empty.");
            }
            context.TraceIdentifier = correlationId.ToString()!;
        }
        else
        {
            context.TraceIdentifier = context.TraceIdentifier.Replace(":", "").ToLowerInvariant();
        }

        context.Items[_options.Key] = context.TraceIdentifier;

        if (_options.IncludeInResponseHeader)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add(_options.Key, new[] { context.TraceIdentifier });
                return Task.CompletedTask;
            });
        }

        if (!_options.IncludeInUserClaim) return _next(context);
        if (context.User.Identity is ClaimsIdentity user && !user.HasClaim(x => x.Type == _options.Key)) 
            user.AddClaim(new Claim(_options.Key, context.TraceIdentifier, ClaimValueTypes.String));

        return _next(context);
    }
}