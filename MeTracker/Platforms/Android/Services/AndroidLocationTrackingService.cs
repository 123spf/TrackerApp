using Android.Content;
using MeTracker.Platforms.Android.Services;

namespace MeTracker.Services;

/// <summary>
/// Contains the Android-specific implementation for the location tracking service.
/// </summary>
public partial class LocationTrackingService
{
    /// <summary>
    /// Initiates background location tracking on the Android platform.
    /// Creates an intent and starts the native AndroidLocationService as a foreground service.
    /// </summary>
    partial void StartTrackingInternal()
    {
        var intent = new Intent(Platform.AppContext, typeof(AndroidLocationService));
        Platform.AppContext.StartForegroundService(intent);
    }
}