using Android.App;
using Android.Content;
using Android.OS;
using Android.Locations;
using MeTracker.Repositories;
using Android.Content.PM;

namespace MeTracker.Platforms.Android.Services;

[Service(ForegroundServiceType = ForegroundService.TypeLocation)]
public class AndroidLocationService : Service, ILocationListener
{
    private ILocationRepository _locationRepository;
    private LocationManager _locationManager;
    public const int NOTIFICATION_ID = 1001;
    public const string CHANNEL_ID = "MeTrackerLocationChannel";

    public override IBinder OnBind(Intent intent) => null;

    public override void OnCreate()
    {
        base.OnCreate();
        _locationRepository = IPlatformApplication.Current.Services.GetRequiredService<ILocationRepository>();
        _locationManager = (LocationManager)GetSystemService(LocationService);
    }

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        CreateNotificationChannel();

        var notification = new Notification.Builder(this, CHANNEL_ID)
            .SetContentTitle("MeTracker Active")
            .SetContentText("Tracking your location in the background.")
            .SetSmallIcon(Resource.Mipmap.appicon) // Ensure you have a default icon here
            .SetOngoing(true)
            .Build();

        StartForeground(NOTIFICATION_ID, notification);

        _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 5000, 5, this); // 5 seconds, 5 meters

        return StartCommandResult.Sticky;
    }
    
    public async void OnLocationChanged(global::Android.Locations.Location location)
    {
        if (location != null)
        {
            await _locationRepository.SaveAsync(new Models.Location(location.Latitude, location.Longitude));
        }
    }

    public override void OnDestroy()
    {
        _locationManager?.RemoveUpdates(this);
        base.OnDestroy();
    }
    
    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;

        var channel = new NotificationChannel(CHANNEL_ID, "Location Tracking", NotificationImportance.Default);
        var notificationManager = (NotificationManager)GetSystemService(NotificationService);
        notificationManager.CreateNotificationChannel(channel);
    }
    
    // Unused ILocationListener methods
    public void OnProviderDisabled(string provider) { }
    public void OnProviderEnabled(string provider) { }
    public void OnStatusChanged(string provider, Availability status, Bundle extras) { }
}