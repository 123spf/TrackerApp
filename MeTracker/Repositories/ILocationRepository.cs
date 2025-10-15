// Path: /Repositories/ILocationRepository.cs

// Use an alias for your model to prevent ambiguity.
using DbLocation = MeTracker.Models.Location;

namespace MeTracker.Repositories;

public interface ILocationRepository
{
    // Use the alias for clean, unambiguous code.
    Task<List<DbLocation>> GetAllAsync();

    Task SaveAsync(DbLocation location);
}