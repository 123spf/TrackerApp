using Microsoft.Extensions.Logging;
using MeTracker.Repositories;
using MeTracker.Services;
using MeTracker.ViewModels;
using MeTracker.Views;

namespace MeTracker;

/// <summary>
/// The main entry point for the application, responsible for configuring and bootstrapping the app.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Creates and configures the .NET MAUI application instance.
    /// </summary>
    /// <returns>A configured MauiApp instance.</returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        // Register services with the dependency injection container.
        // Singleton services are created once and shared throughout the app's lifetime.
        builder.Services.AddSingleton<ILocationTrackingService, LocationTrackingService>();
        builder.Services.AddSingleton<ILocationRepository, LocationRepository>();

        // Transient services are created each time they are requested from the container.
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainView>();

        return builder.Build();
    }
}