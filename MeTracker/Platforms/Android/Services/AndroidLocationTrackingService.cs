using Android.Content;
using MeTracker.Platforms.Android.Services;

namespace MeTracker.Services;

public partial class LocationTrackingService
{
    partial void StartTrackingInternal()
    {
        var intent = new Intent(MauiApplication.Current.ApplicationContext, typeof(AndroidLocationService));
        MauiApplication.Current.ApplicationContext.StartForegroundService(intent);
    }
}