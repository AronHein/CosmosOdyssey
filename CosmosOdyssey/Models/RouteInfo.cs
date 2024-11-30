namespace Models;

public class RouteInfo
{
    public string Id { get; set; } = default!;
    public Location From { get; set; } = default!;
    public Location To { get; set; } = default!;
    public double Distance { get; set; }
}