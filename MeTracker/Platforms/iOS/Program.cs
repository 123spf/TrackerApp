using UIKit;

namespace MeTracker;

/// <summary>
/// Contains the main entry point for the application on iOS and Mac Catalyst.
/// </summary>
public class Program
{
    /// <summary>
    /// Serves as the initial entry point when the application is launched on Apple platforms.
    /// </summary>
    /// <param name="args">An array of command-line arguments.</param>
    static void Main(string[] args)
    {
        // This call starts the native iOS application and specifies the main application delegate class.
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}