
namespace MeTracker.Models;

public class Point
{
    public Models.Location Location { get; set; }
    public int Count { get; set; } = 1;
    public Color Heat { get; set; }

}