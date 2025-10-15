namespace MeTracker.Services;

/// <summary>
/// Provides the shared, cross-platform implementation for the location tracking service.
/// It uses a partial method to invoke the platform-specific logic.
/// </summary>
public partial class LocationTrackingService : ILocationTrackingService
{
	/// <summary>
	/// Starts the location tracking process by calling the platform-specific implementation.
	/// </summary>
	public void StartTracking()
	{
		StartTrackingInternal();
	}

	/// <summary>
	/// Defines the platform-specific entry point for starting the tracking service.
	/// </summary>
	/// <remarks>
	/// The implementation for this method is provided in the platform-specific
	/// partial class files (e.g., Platforms/Android/LocationTrackingService.cs).
	/// </remarks>
	partial void StartTrackingInternal();
}