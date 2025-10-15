// Path: /Repositories/LocationRepository.cs

using SQLite;
using System.Diagnostics; // Add this for logging
// Use the same alias here.
using DbLocation = MeTracker.Models.Location;

namespace MeTracker.Repositories;

public class LocationRepository : ILocationRepository
{
    private SQLiteAsyncConnection? connection;

    public async Task<List<DbLocation>> GetAllAsync()
    {
        await CreateConnectionAsync();
        var locations = await connection!.Table<DbLocation>().ToListAsync();
        // --- LOG ADDED ---
        // Log how many locations were retrieved from the database.
        Debug.WriteLine($"[LocationRepository] GetAllAsync: Found {locations.Count} locations in the database.");
        return locations;
    }

    public async Task SaveAsync(DbLocation location)
    {
        await CreateConnectionAsync();
        await connection!.InsertAsync(location);
        // --- LOG ADDED ---
        // Log that a new location has been saved.
        Debug.WriteLine($"[LocationRepository] SaveAsync: Saved location Lat: {location.Latitude}, Lon: {location.Longitude}");
    }
    
    private async Task CreateConnectionAsync()
    {
        if (connection != null)
        {
            return;
        }

        var databasePath = Path.Combine(
            FileSystem.AppDataDirectory,
            "Locations.db");

        // --- LOG ADDED ---
        // Log the path to the database to ensure it's correct.
        Debug.WriteLine($"[LocationRepository] Database path: {databasePath}");

        connection = new SQLiteAsyncConnection(databasePath);
        await connection.CreateTableAsync<DbLocation>();
    }
}
