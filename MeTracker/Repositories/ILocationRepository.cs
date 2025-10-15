// Use an alias to avoid naming conflicts with other 'Location' types.
using DbLocation = MeTracker.Models.Location;

namespace MeTracker.Repositories;

/// <summary>
/// Defines the contract for a repository that handles data access operations for user locations.
/// </summary>
public interface ILocationRepository
{
    /// <summary>
    /// Asynchronously retrieves all saved location records from the data store.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation and contains a list of all locations.</returns>
    Task<List<DbLocation>> GetAllAsync();

    /// <summary>
    /// Asynchronously saves a single location record to the data store.
    /// </summary>
    /// <param name="location">The location object to save.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveAsync(DbLocation location);
}