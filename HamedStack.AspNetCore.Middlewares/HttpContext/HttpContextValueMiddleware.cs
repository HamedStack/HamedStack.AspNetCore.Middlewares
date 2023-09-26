// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

using Microsoft.AspNetCore.Http;

namespace HamedStack.AspNetCore.Middlewares.HttpContext;

/// <summary>
/// Middleware to extract a value from the HTTP context and set it using a specific service.
/// </summary>
/// <typeparam name="TService">The type of the service.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class HttpContextValueMiddleware<TService, TValue>
    where TService : IHttpContextValue<TValue>
{
    private readonly RequestDelegate _next;
    private readonly Func<Microsoft.AspNetCore.Http.HttpContext, TValue> _valueExtractor;
    private readonly TService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpContextValueMiddleware{TService, TValue}"/> class.
    /// </summary>
    /// <param name="next">The next middleware delegate in the pipeline.</param>
    /// <param name="valueExtractor">A function to extract the value from the HTTP context.</param>
    /// <param name="service">The service to set the extracted value.</param>
    public HttpContextValueMiddleware(RequestDelegate next, Func<Microsoft.AspNetCore.Http.HttpContext, TValue> valueExtractor, TService service)
    {
        _next = next;
        _valueExtractor = valueExtractor;
        _service = service;
    }

    /// <summary>
    /// Processes a request to extract a value from the HTTP context and set it using the specified service.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
    {
        var value = _valueExtractor(context);
        _service.Set(value);
        await _next(context);
    }
}