using SQLite;
using System.Diagnostics;
// Use an alias to avoid naming conflicts with other 'Location' types.
using DbLocation = MeTracker.Models.Location;

namespace MeTracker.Repositories;

/// <summary>
/// Implements the location repository contract using an SQLite database for data persistence.
/// </summary>
public class LocationRepository : ILocationRepository
{
    /// <summary>
    /// Holds the asynchronous connection to the SQLite database.
    /// </summary>
    private SQLiteAsyncConnection? connection;

    /// <summary>
    /// Asynchronously retrieves all saved location records from the data store.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation and contains a list of all locations.</returns>
    public async Task<List<DbLocation>> GetAllAsync()
    {
        await CreateConnectionAsync();
        var locations = await connection!.Table<DbLocation>().ToListAsync();
        Debug.WriteLine($"[LocationRepository] GetAllAsync: Found {locations.Count} locations in the database.");
        return locations;
    }

    /// <summary>
    /// Asynchronously saves a single location record to the data store.
    /// </summary>
    /// <param name="location">The location object to save.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync(DbLocation location)
    {
        await CreateConnectionAsync();
        await connection!.InsertAsync(location);
        Debug.WriteLine($"[LocationRepository] SaveAsync: Saved location Lat: {location.Latitude}, Lon: {location.Longitude}");
    }
    
    /// <summary>
    /// Initializes the database connection on the first request.
    /// Ensures the connection is only created once per repository instance.
    /// </summary>
    private async Task CreateConnectionAsync()
    {
        if (connection != null)
        {
            return;
        }

        var databasePath = Path.Combine(
            FileSystem.AppDataDirectory,
            "Locations.db");
        
        Debug.WriteLine($"[LocationRepository] Database path: {databasePath}");

        connection = new SQLiteAsyncConnection(databasePath);
        await connection.CreateTableAsync<DbLocation>();
    }
}