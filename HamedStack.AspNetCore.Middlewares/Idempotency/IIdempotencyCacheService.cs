// ReSharper disable IdentifierTypo

namespace HamedStack.AspNetCore.Middlewares.Idempotency;

/// <summary>
/// Provides an interface for caching idempotent request responses.
/// </summary>
public interface IIdempotencyCacheService
{
    /// <summary>
    /// Retrieves a cached response based on a key.
    /// </summary>
    /// <param name="key">The key to lookup the cached response.</param>
    /// <returns>A cached response if found, null otherwise.</returns>
    Task<IdempotencyCachedResponse?> GetCacheResponseAsync(string key);

    /// <summary>
    /// Sets a cached response with an optional expiration time.
    /// </summary>
    /// <param name="key">The key to associate with the cached response.</param>
    /// <param name="response">The response data to cache.</param>
    /// <param name="expirationTime">Optional expiration time for the cached response.</param>
    Task SetCacheResponseAsync(string key, IdempotencyCachedResponse response, TimeSpan? expirationTime = null);

    /// <summary>
    /// Checks if the request can proceed based on the provided rate limit.
    /// </summary>
    /// <param name="key">The key associated with the request.</param>
    /// <param name="limit">The rate limit to check against.</param>
    /// <returns>True if the request can proceed, false otherwise.</returns>
    Task<bool> CanProceedWithRequest(string key, int limit);
}