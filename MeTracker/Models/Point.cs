namespace MeTracker.Models;

/// <summary>
/// Represents an aggregated data point used for heat map visualization,
/// grouping multiple nearby location records.
/// </summary>
public class Point
{
    /// <summary>
    /// Gets or sets the representative geographical coordinate for the aggregated point.
    /// </summary>
    public Location? Location { get; set; }

    /// <summary>
    /// Gets or sets the number of individual locations grouped into this point,
    /// representing the frequency of visits.
    /// </summary>
    public int Count { get; set; } = 1;

    /// <summary>
    /// Gets or sets the calculated color for the point's visualization on the heat map.
    /// </summary>
    public Color? Heat { get; set; }
}