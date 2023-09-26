// ReSharper disable IdentifierTypo
namespace HamedStack.AspNetCore.Middlewares.RequestTiming;


/// <summary>
/// Represents the options for limiting the size of incoming requests.
/// </summary>
public class RequestSizeLimitOptions
{
    /// <summary>
    /// Gets or sets the maximum allowed size for incoming requests in bytes.
    /// </summary>
    public long MaxBytes { get; set; }
}