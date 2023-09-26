// ReSharper disable IdentifierTypo
namespace HamedStack.AspNetCore.Middlewares.CustomHeader;

/// <summary>
/// Represents options for custom header configuration.
/// </summary>
public class CustomHeaderOptions
{
    /// <summary>
    /// Gets or sets the header name.
    /// </summary>
    public string HeaderName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the expected value for the header.
    /// </summary>
    public string HeaderExpectedValue { get; set; } = null!;
}