using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace MeTracker.Controls;

/// <summary>
/// Extends the Map control to visualize a collection of location data points as a heat map.
/// </summary>
public class CustomMap : Map
{
    /// <summary>
    /// Defines a bindable property for the collection of data points to be displayed on the map.
    /// </summary>
    public readonly static BindableProperty PointsProperty = BindableProperty.Create(nameof(Points), typeof(List<Models.Point>), typeof(CustomMap), new List<Models.Point>(), propertyChanged: OnPointsChanged);

    /// <summary>
    /// Handles changes to the Points property by converting the data points into visual map elements.
    /// </summary>
    /// <param name="bindable">The map control that has the changed property.</param>
    /// <param name="oldValue">The previous list of points.</param>
    /// <param name="newValue">The new list of points to be rendered.</param>
    private static void OnPointsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var map = bindable as Map;
        if (newValue == null) return;
        if (map is null) return;
        var points = newValue as List<Models.Point>;
        if (points == null) return;
        foreach (var point in points)
        {
            if (point.Location is null) continue;

            Circle circle = new()
            {
                Center = new Location(point.Location.Latitude, point.Location.Longitude),
                Radius = new Distance(200),
                StrokeColor = Color.FromArgb("#88FF0000"),
                StrokeWidth = 0,
                FillColor = point.Heat ?? Colors.Transparent
            };
            
            map.MapElements.Add(circle);
        }
    }

    /// <summary>
    /// Gets or sets the list of points to display on the map.
    /// </summary>
    public List<Models.Point> Points
    {
        get => (GetValue(PointsProperty) as List<Models.Point>) ?? new List<Models.Point>();
        set => SetValue(PointsProperty, value);
    }

    /// <summary>
    /// Initializes the map with default settings.
    /// </summary>
    public CustomMap()
    {
        IsScrollEnabled = true;
        IsShowingUser = true;
    }
}