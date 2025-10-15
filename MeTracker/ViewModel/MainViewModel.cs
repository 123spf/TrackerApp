using CommunityToolkit.Mvvm.ComponentModel;
using MeTracker.Repositories;
using System.Diagnostics;
// Use an alias to avoid naming conflicts with other 'Location' types.
using MauiLocation = Microsoft.Maui.Devices.Sensors.Location;

namespace MeTracker.ViewModels;

/// <summary>
/// Manages the data and logic for the main view.
/// Processes raw location data into a collection of points for heat map display.
/// </summary>
public partial class MainViewModel : ViewModel
{
    private readonly ILocationRepository locationRepository;

    /// <summary>
    /// Gets or sets the collection of processed data points for the heat map.
    /// This property is bindable and will notify the UI of any changes.
    /// </summary>
    [ObservableProperty]
    private List<Models.Point> points = new List<Models.Point>();

    /// <summary>
    /// Initializes a new instance of the MainViewModel.
    /// </summary>
    /// <param name="locationRepository">The repository for accessing location data.</param>
    public MainViewModel(ILocationRepository locationRepository)
    {
        this.locationRepository = locationRepository;
    }

    /// <summary>
    /// Loads raw location data from the repository, processes it by grouping nearby
    /// coordinates into points, calculates a heat value for each point based on visit
    /// frequency, and updates the public Points property to refresh the UI.
    /// </summary>
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

        // Group all retrieved locations into points based on proximity (200m radius).
        foreach (var dbLocation in locations)
        {
            var existingPoint = pointList.FirstOrDefault(p => p.Location != null &&
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

        if (!pointList.Any())
        {
            Debug.WriteLine("[MainViewModel] LoadDataAsync: No points were created after grouping.");
             return;
        }

        // Calculate a "heat" color for each point based on its visit count.
        var maxCount = pointList.Max(p => p.Count);
        var minCount = pointList.Min(p => p.Count);
        var diff = (float)(maxCount - minCount);

        foreach (var point in pointList)
        {
            float normalized = (diff > 0) ? ((float)point.Count - minCount) / diff : 0;
            // A hue from 0 (red/hot) to 0.66 (blue/cold) is calculated based on visit frequency.
            var hue = (1 - normalized) * (2f / 3f); 
            point.Heat = Color.FromHsla(hue, 1, 0.5);
        }
        
        Debug.WriteLine($"[MainViewModel] LoadDataAsync: Finished processing. Updating UI with {pointList.Count} points.");
        Points = pointList;
    }
}