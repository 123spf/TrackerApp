using CoreLocation;
using MeTracker.Repositories;

namespace MeTracker.Services;

/// <summary>
/// Contains the iOS specific implementation for the location tracking service.
/// </summary>
public partial class LocationTrackingService : ILocationTrackingService
{
    CLLocationManager? locationManager;
    readonly ILocationRepository locationRepository;

    /// <summary>
    /// Initializes a new instance of the LocationTrackingService.
    /// </summary>
    /// <param name="locationRepository">The repository used for saving location data.</param>
    public LocationTrackingService(ILocationRepository locationRepository)
    {
        this.locationRepository = locationRepository;
    }
    
    /// <summary>
    /// Configures and starts the native CoreLocation manager to begin tracking the user's location.
    /// </summary>
    partial void StartTrackingInternal()
    {
        // Configure the location manager for high accuracy and continuous background updates.
        locationManager = new CLLocationManager
        {
            PausesLocationUpdatesAutomatically = false,
            DesiredAccuracy = CLLocation.AccuracyBestForNavigation,
            AllowsBackgroundLocationUpdates = true,
        };

        // Subscribe to location updates to save new coordinates as they are received.
        locationManager.LocationsUpdated += async (object? sender, CLLocationsUpdatedEventArgs e) =>
        {
            var lastLocation = e.Locations.Last();
            var newLocation = new Models.Location(lastLocation.Coordinate.Latitude, lastLocation.Coordinate.Longitude);
            await locationRepository.SaveAsync(newLocation);
        };
        
        // Request permission from the user and begin receiving location updates.
        locationManager.RequestAlwaysAuthorization();
        locationManager.StartUpdatingLocation();
    }
}