using MeTracker.Services;
using MeTracker.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace MeTracker.Views;
public partial class MainView : ContentPage
{
    // --- CHANGE #1: STORE REFERENCES ---
    // Store references to the services we need during the page's lifecycle.
    private readonly MainViewModel viewModel;
    private readonly ILocationTrackingService locationTrackingService;

	public MainView(MainViewModel viewModel, ILocationTrackingService locationTrackingService)
	{
		InitializeComponent();
        
        // --- CHANGE #2: SIMPLIFIED CONSTRUCTOR ---
        // The constructor now only sets up the references and the BindingContext.
        // All logic is moved out to prevent running it too early.
		this.viewModel = viewModel;
        this.locationTrackingService = locationTrackingService;
		BindingContext = this.viewModel;
	}

    // --- CHANGE #3: USE THE OnAppearing LIFECYCLE EVENT ---
    // This method is called by the .NET MAUI framework automatically
    // when the page is displayed on screen. This is the safe place
    // to start the foreground service.
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync();

        if (status == PermissionStatus.Granted)
        {
            // 1. Start the tracking service safely.
            locationTrackingService.StartTracking();

            // 2. Load any previously saved locations from the database.
            await viewModel.LoadDataAsync();

            // 3. Center the map on the user's current location.
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
