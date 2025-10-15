using Android.App;
using Android.Content;
using Android.OS;
using Android.Locations;
using MeTracker.Repositories;
using Android.Content.PM;

namespace MeTracker.Platforms.Android.Services;

/// <summary>
/// Implements a foreground service for persistent background location tracking on Android.
/// Listens for location updates and saves them to the repository.
/// </summary>
[Service(ForegroundServiceType = ForegroundService.TypeLocation)]
public class AndroidLocationService : Service, ILocationListener
{
    private ILocationRepository? _locationRepository;
    private LocationManager? _locationManager;

    /// <summary>
    /// The unique identifier for the foreground service notification.
    /// </summary>
    public const int NOTIFICATION_ID = 1001;

    /// <summary>
    /// The identifier for the notification channel used by this service.
    /// </summary>
    public const string CHANNEL_ID = "MeTrackerLocationChannel";

    /// <summary>
    /// This service does not support binding, so it returns null.
    /// </summary>
    public override IBinder? OnBind(Intent? intent) => null;

    /// <summary>
    /// Performs one-time setup when the service is first created.
    /// Resolves dependencies and acquires the system location manager.
    /// </summary>
    public override void OnCreate()
    {
        base.OnCreate();
        _locationRepository = IPlatformApplication.Current!.Services.GetRequiredService<ILocationRepository>();
        _locationManager = GetSystemService(LocationService) as LocationManager;
    }

    /// <summary>
    /// Called when the service is started.
    /// Promotes the service to the foreground with a persistent notification and requests location updates.
    /// </summary>
    /// <returns>A value indicating how the system should handle the service if it's killed.</returns>
    public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
    {
        CreateNotificationChannel();

        var notification = new Notification.Builder(this, CHANNEL_ID)
            .SetContentTitle("MeTracker Active")
            .SetContentText("Tracking your location in the background.")
            .SetSmallIcon(Resource.Mipmap.appicon) 
            .SetOngoing(true)
            .Build();

        StartForeground(NOTIFICATION_ID, notification);

        _locationManager?.RequestLocationUpdates(LocationManager.GpsProvider, 5000, 5, this);

        return StartCommandResult.Sticky;
    }
    
    /// <summary>
    /// Handles new location data by saving it to the database.
    /// </summary>
    /// <param name="location">The new location received from the system.</param>
    public async void OnLocationChanged(global::Android.Locations.Location location)
    {
        if (location != null && _locationRepository != null)
        {
            await _locationRepository.SaveAsync(new Models.Location(location.Latitude, location.Longitude));
        }
    }

    /// <summary>
    /// Cleans up resources when the service is destroyed.
    /// </summary>
    public override void OnDestroy()
    {
        _locationManager?.RemoveUpdates(this);
        base.OnDestroy();
    }
    
    /// <summary>
    /// Creates a notification channel, required for notifications on Android 8.0 (Oreo) and higher.
    /// </summary>
    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;

        var channel = new NotificationChannel(CHANNEL_ID, "Location Tracking", NotificationImportance.Default);
        var notificationManager = GetSystemService(NotificationService) as NotificationManager;
        notificationManager?.CreateNotificationChannel(channel);
    }
    
    // These methods are required by the ILocationListener interface but are not used in this implementation.
    public void OnProviderDisabled(string? provider) { }
    public void OnProviderEnabled(string? provider) { }
    public void OnStatusChanged(string? provider, Availability status, Bundle? extras) { }
}