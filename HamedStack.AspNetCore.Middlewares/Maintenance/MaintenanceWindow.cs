// ReSharper disable IdentifierTypo
namespace HamedStack.AspNetCore.Middlewares.Maintenance;

/// <summary>
/// Configuration for maintenance mode.
/// </summary>
public class MaintenanceWindow
{
    private readonly Func<bool> _enabledFunc;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaintenanceWindow"/> class.
    /// </summary>
    /// <param name="enabledFunc">A function that determines if maintenance mode is enabled.</param>
    /// <param name="response">The response to provide during maintenance mode.</param>
    public MaintenanceWindow(Func<bool> enabledFunc, byte[] response)
    {
        _enabledFunc = enabledFunc;
        Response = response;
    }

    /// <summary>
    /// Gets a value indicating whether maintenance mode is enabled.
    /// </summary>
    public bool Enabled => _enabledFunc();

    /// <summary>
    /// Gets the response content to be served during maintenance mode.
    /// </summary>
    public byte[] Response { get; }

    /// <summary>
    /// Gets or sets the time, in seconds, after which the client should retry.
    /// </summary>
    public int? RetryAfterInSeconds { get; set; } = null;

    /// <summary>
    /// Gets or sets the content type of the maintenance response.
    /// </summary>
    public string ContentType { get; set; } = "text/html";
}