using MeTracker.Services;
using MeTracker.ViewModels;
using Microsoft.Maui.Maps;

namespace MeTracker.Views;

/// <summary>
/// Represents the main user interface page, responsible for displaying the location map.
/// </summary>
public partial class MainView : ContentPage
{
    private readonly MainViewModel viewModel;
    private readonly ILocationTrackingService locationTrackingService;

    /// <summary>
    /// Initializes a new instance of the MainView, injecting required services.
    /// </summary>
    /// <param name="viewModel">The ViewModel containing the view's data and logic.</param>
    /// <param name="locationTrackingService">The service for managing location tracking.</param>
	public MainView(MainViewModel viewModel, ILocationTrackingService locationTrackingService)
	{
		InitializeComponent();
        
        // Store service references and set the binding context to link the View to the ViewModel.
		this.viewModel = viewModel;
        this.locationTrackingService = locationTrackingService;
		BindingContext = this.viewModel;
	}

    /// <summary>
    /// Executes logic when the page is displayed on screen.
    /// Handles permission checks, starts the tracking service, loads data, and centers the map.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync();

        if (status == PermissionStatus.Granted)
        {
            // Start the background location tracking service.
            locationTrackingService.StartTracking();

            // Instruct the ViewModel to load and process existing location data for the heat map.
            await viewModel.LoadDataAsync();

            // Center the map on the user's current or last known location.
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));
            }

            if (location is not null)
            {
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(2)));
            }
        }
    }
}