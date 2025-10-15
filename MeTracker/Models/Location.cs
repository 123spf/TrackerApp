using SQLite;

namespace MeTracker.Models;

/// <summary>
/// Represents the data model for a single geographical location.
/// </summary>
public class Location
{
    /// <summary>
    /// Initializes a new, empty instance of the Location class.
    /// </summary>
    public Location() { }

    /// <summary>
    /// Initializes a new instance of the Location class with specific coordinates.
    /// </summary>
    /// <param name="latitude">The latitude of the location.</param>
    /// <param name="longitude">The longitude of the location.</param>
    public Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Gets or sets the unique identifier for the location in the database.
    /// </summary>
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the latitude of the geographical coordinate.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude of the geographical coordinate.
    /// </summary>
    public double Longitude { get; set; }
}