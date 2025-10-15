using Android.App;
using Android.Runtime;

// Declares the necessary Android permissions for the application.
// These permissions are required for accessing location data, network state, and starting services on boot.
[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessWifiState)]
[assembly: UsesPermission(Android.Manifest.Permission.ReceiveBootCompleted)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]

namespace MeTracker;

/// <summary>
/// Represents the main entry point and application class for the Android app.
/// </summary>
[Application]
public class MainApplication : MauiApplication
{
    /// <summary>
    /// Initializes a new instance of the MainApplication class.
    /// This constructor is required for Android runtime object creation.
    /// </summary>
    /// <param name="handle">A handle to the underlying Android object.</param>
    /// <param name="ownership">Specifies the JNI handle ownership.</param>
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    /// <summary>
    /// Creates and initializes the cross-platform .NET MAUI application.
    /// </summary>
    /// <returns>The configured .NET MAUI application instance.</returns>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}