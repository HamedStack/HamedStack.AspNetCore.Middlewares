// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using Microsoft.AspNetCore.Builder;

namespace HamedStack.AspNetCore.Middlewares.GlobalException;

/// <summary>
/// Contains the extension method for configuring the <see cref="GlobalExceptionMiddleware"/>.
/// </summary>
public static class GlobalExceptionMiddlewareExtensions
{
    /// <summary>
    /// Adds the <see cref="GlobalExceptionMiddleware"/> to the specified application builder.
    /// </summary>
    /// <param name="app">The application builder to add the middleware to.</param>
    /// <returns>The application builder with the middleware added.</returns>
    public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}