namespace Models;

public class RouteInfo
{
    public Guid Id { get; set; }
    public Location From { get; set; }
    public Location To { get; set; }
    public double Distance { get; set; }
}