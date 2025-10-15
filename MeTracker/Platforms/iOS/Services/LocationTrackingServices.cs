using CoreLocation;
using MeTracker.Repositories;
using MeTracker.Models;

namespace MeTracker.Services;

public partial class LocationTrackingService : ILocationTrackingService
{
    CLLocationManager? locationManager;
    ILocationRepository locationRepository;

    public LocationTrackingService(ILocationRepository locationRepository)
    {
        this.locationRepository = locationRepository;
    }
    
    partial void StartTrackingInternal()
    {
        locationManager = new CLLocationManager
        {
            PausesLocationUpdatesAutomatically = false,
            DesiredAccuracy = CLLocation.AccuracyBestForNavigation,
            AllowsBackgroundLocationUpdates = true,
        };

        locationManager.LocationsUpdated += async (object? sender, CLLocationsUpdatedEventArgs e) =>
        {
            var lastLocation = e.Locations.Last();
            var newLocation = new Models.Location(lastLocation.Coordinate.Latitude, lastLocation.Coordinate.Longitude);
            await locationRepository.SaveAsync(newLocation);
        };
        
        locationManager.RequestAlwaysAuthorization();
        locationManager.StartUpdatingLocation();
    }
}