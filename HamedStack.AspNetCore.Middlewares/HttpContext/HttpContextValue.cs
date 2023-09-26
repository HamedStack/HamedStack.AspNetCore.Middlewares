// ReSharper disable IdentifierTypo
namespace HamedStack.AspNetCore.Middlewares.HttpContext;

/// <summary>
/// Provides a generic implementation of <see cref="IHttpContextValue{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class HttpContextValue<T> : IHttpContextValue<T>
{
    /// <inheritdoc/>
    public T? Value { get; private set; }

    /// <inheritdoc/>
    public void Set(T? value)
    {
        Value = value;
    }
}