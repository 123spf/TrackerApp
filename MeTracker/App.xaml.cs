namespace MeTracker;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    // This method replaces setting MainPage in the constructor.
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    // This method uses the modern API to refresh the UI.
    protected override void OnResume()
    {
        base.OnResume();

            // For a single-window app, this sets the page on the main window.
        Windows[0].Page = new AppShell();

    }
}