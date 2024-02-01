using Microsoft.AspNetCore.Builder;

namespace HamedStack.AspNetCore.Middlewares.LogHeaders;

/// <summary>
/// Extension methods for adding the LogHeadersMiddleware to the application's request pipeline.
/// </summary>
public static class LogHeadersMiddlewareExtensions
{
    /// <summary>
    /// Adds the LogHeadersMiddleware to the application's request pipeline.
    /// </summary>
    /// <param name="builder">The IApplicationBuilder instance.</param>
    /// <returns>The IApplicationBuilder instance with the middleware configured.</returns>
    public static IApplicationBuilder UseLogHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogHeadersMiddleware>();
    }
}
