// Path: /ViewModels/MainViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeTracker.Repositories;
using MeTracker.Services;
using System.Diagnostics;
using MauiLocation = Microsoft.Maui.Devices.Sensors.Location;

namespace MeTracker.ViewModels;

public partial class MainViewModel : ViewModel
{
    private readonly ILocationRepository locationRepository;

    [ObservableProperty]
    private List<Models.Point> points = new List<Models.Point>();

    // --- CHANGE #1: SIMPLIFIED CONSTRUCTOR ---
    // The ILocationTrackingService is no longer needed here.
    // The logic to start the service and load data is moved to the View's lifecycle.
    public MainViewModel(ILocationRepository locationRepository)
    {
        this.locationRepository = locationRepository;
    }

    // --- CHANGE #2: METHOD MADE PUBLIC ---
    // This is now public so the MainView's code-behind can call it
    // when the page appears on screen.
    public async Task LoadDataAsync()
    {
        Debug.WriteLine("[MainViewModel] LoadDataAsync: Attempting to get all locations from repository.");
        var locations = await locationRepository.GetAllAsync();

        if (locations == null || !locations.Any())
        {
            Debug.WriteLine("[MainViewModel] LoadDataAsync: No locations found. Setting points to empty list.");
            Points = new List<Models.Point>();
            return;
        }
        
        Debug.WriteLine($"[MainViewModel] LoadDataAsync: Processing {locations.Count} locations into points.");

        var pointList = new List<Models.Point>();

        // Step 1: Group all locations into points
        foreach (var dbLocation in locations)
        {
            var existingPoint = pointList.FirstOrDefault(p =>
                MauiLocation.CalculateDistance(
                    p.Location.Latitude, p.Location.Longitude,
                    dbLocation.Latitude, dbLocation.Longitude,
                    DistanceUnits.Kilometers) < 0.2);

            if (existingPoint != null)
            {
                existingPoint.Count++;
            }
            else
            {
                pointList.Add(new Models.Point() { Location = dbLocation });
            }
        }

        // Step 2: Calculate heat for each point after grouping is complete
        if (!pointList.Any())
        {
            Debug.WriteLine("[MainViewModel] LoadDataAsync: No points were created after grouping.");
             return;
        }

        var maxCount = pointList.Max(p => p.Count);
        var minCount = pointList.Min(p => p.Count);
        var diff = (float)(maxCount - minCount);

        foreach (var point in pointList)
        {
            float normalized = (diff > 0) ? ((float)point.Count - minCount) / diff : 0;
            var hue = (1 - normalized) * (2f / 3f); 
            point.Heat = Color.FromHsla(hue, 1, 0.5);
        }
        
        Debug.WriteLine($"[MainViewModel] LoadDataAsync: Finished processing. Updating UI with {pointList.Count} points.");
        Points = pointList;
    }
}

