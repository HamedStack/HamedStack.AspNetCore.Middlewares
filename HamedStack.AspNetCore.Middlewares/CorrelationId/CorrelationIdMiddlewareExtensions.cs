// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HamedStack.AspNetCore.Middlewares.CorrelationId;

/// <summary>
/// Contains extension methods for configuring and using the <see cref="CorrelationIdMiddleware"/>.
/// </summary>
public static class CorrelationIdMiddlewareExtensions
{
    /// <summary>
    /// Adds services and configuration for the correlation ID.
    /// </summary>
    /// <param name="service">The service collection to add services to.</param>
    /// <param name="options">Optional configuration for the correlation ID.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddCorrelationId(this IServiceCollection service, Action<CorrelationIdOption>? options = default)
    {
        options ??= _ => { };

        service.Configure(options);
        return service;
    }

    /// <summary>
    /// Adds the <see cref="CorrelationIdMiddleware"/> to the specified application builder.
    /// </summary>
    /// <param name="builder">The application builder to add the middleware to.</param>
    /// <returns>The application builder with the middleware added.</returns>
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationIdMiddleware>();
    }
}