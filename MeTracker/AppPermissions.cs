namespace MeTracker;

/// <summary>
/// Handles checking and requesting app permissions at runtime.
/// </summary>
internal partial class AppPermissions
{
    /// <summary>
    /// Defines a custom permission type that derives from the default location permission.
    /// </summary>
    internal partial class AppPermission : Permissions.LocationWhenInUse
    {
    }

    /// <summary>
    /// Retrieves the current status of the required location permission.
    /// </summary>
    /// <returns>The current status of the permission.</returns>
    public static async Task<PermissionStatus> CheckRequiredPermissionAsync() => await Permissions.CheckStatusAsync<AppPermission>();

    /// <summary>
    /// Manages the process of checking for and requesting location permission from the user.
    /// </summary>
    /// <returns>The final status of the permission after the check and/or request.</returns>
    public static async Task<PermissionStatus> CheckAndRequestRequiredPermissionAsync()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<AppPermission>();

        if (status == PermissionStatus.Granted)
            return status;

        // On iOS, if permission has been denied, instructs the user how to grant it in the settings panel.
        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            var mainPage = Application.Current?.Windows[0]?.Page;
            if (mainPage is not null)
            {
                await mainPage.DisplayAlert("Required App Permissions", "Please enable all permissions in Settings for this App, it is useless without them.", "Ok");
            }
        }

        // Displays a rationale for the permission request if the system recommends it.
        if (Permissions.ShouldShowRationale<AppPermission>())
        {
            var mainPage = Application.Current?.Windows[0]?.Page;
            if (mainPage is not null)
            {
                await mainPage.DisplayAlert("Required App Permissions", "This is a location based app, without these permissions it is useless.", "Ok");
            }
        }

        status = await MainThread.InvokeOnMainThreadAsync(Permissions.RequestAsync<AppPermission>);
        return status;
    }
}