// ReSharper disable IdentifierTypo
namespace HamedStack.AspNetCore.Middlewares.TimeRestriction;

/// <summary>
/// Represents the options for restricting requests based on time.
/// </summary>
public class TimeRestrictionOptions
{
    /// <summary>
    /// Gets or sets the start time for allowing requests.
    /// </summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time for allowing requests.
    /// </summary>
    public TimeSpan EndTime { get; set; }
}