// ReSharper disable IdentifierTypo
namespace HamedStack.AspNetCore.Middlewares.HttpContext;

/// <summary>
/// Represents a generic interface for accessing and setting HTTP context values.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public interface IHttpContextValue<T>
{
    /// <summary>
    /// Gets the value from the HTTP context.
    /// </summary>
    T? Value { get; }

    /// <summary>
    /// Sets the value in the HTTP context.
    /// </summary>
    /// <param name="value">The value to set.</param>
    void Set(T? value);
}