// ReSharper disable IdentifierTypo
namespace HamedStack.AspNetCore.Middlewares.Idempotency;

/// <summary>
/// Represents settings for the IdempotencyMiddleware.
/// </summary>
public class IdempotencySettings
{
    /// <summary>
    /// Gets or sets the default expiration time for cached responses.
    /// </summary>
    public TimeSpan? DefaultExpirationTime { get; set; }

    /// <summary>
    /// Gets or sets the rate limit for requests. Null means no limit.
    /// </summary>
    public int? RateLimit { get; set; }
}