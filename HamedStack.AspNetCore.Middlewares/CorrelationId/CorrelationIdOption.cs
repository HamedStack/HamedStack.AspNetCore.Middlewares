// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace HamedStack.AspNetCore.Middlewares.CorrelationId;

/// <summary>
/// Provides configuration options for correlation ID handling.
/// </summary>
public class CorrelationIdOption
{
    /// <summary>
    /// Gets or sets the key used for the correlation ID in headers and claims.
    /// Defaults to "x-correlation-id".
    /// </summary>
    public string Key { get; set; } = "x-correlation-id";

    /// <summary>
    /// Gets or sets a value indicating whether to include the correlation ID in the response header.
    /// Defaults to true.
    /// </summary>
    public bool IncludeInResponseHeader { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to include the correlation ID in the user's claims.
    /// Defaults to true.
    /// </summary>
    public bool IncludeInUserClaim { get; set; } = true;
}