#if NET8_0_OR_GREATER

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HamedStack.AspNetCore.Middlewares.GlobalException;

/// <summary>
/// Contains extension methods for registering and configuring the global exception handler.
/// </summary>
public static class GlobalExceptionHandlerExtensions
{
    /// <summary>
    /// Adds the global exception handler to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method registers the <see cref="GlobalExceptionHandler"/> and problem details services.
    /// </remarks>
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }

    /// <summary>
    /// Configures the application to use the global exception handler.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to configure.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method sets up the application to use the exception handler middleware.
    /// </remarks>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
        return app;
    }
}
#endif
