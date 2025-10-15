namespace MeTracker.Services;

/// <summary>
/// Defines the contract for a service that manages the background tracking of the user's location.
/// </summary>
public interface ILocationTrackingService
{
    /// <summary>
    /// Starts the location tracking process.
    /// </summary>
    void StartTracking();
}