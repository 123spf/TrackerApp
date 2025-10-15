using Android.App;
using Android.Content.PM;

namespace MeTracker;

// Registers the main activity with the Android operating system.
// - Theme: Specifies the splash screen theme to use during app startup.
// - MainLauncher: Designates this activity as the app's entry point.
// - ConfigurationChanges: Informs Android that the app will handle UI changes
//   (like rotation or theme changes) itself, preventing the activity from being recreated.
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}