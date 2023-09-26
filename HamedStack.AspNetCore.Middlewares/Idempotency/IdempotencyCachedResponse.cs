// ReSharper disable IdentifierTypo
namespace HamedStack.AspNetCore.Middlewares.Idempotency;

/// <summary>
/// Represents cached response data for idempotent requests.
/// </summary>
public class IdempotencyCachedResponse
{
    /// <summary>
    /// Gets or sets the HTTP status code for the response.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the response content data.
    /// </summary>
    public string Data { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp of when the response was cached.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }
}