namespace Models;

public class Leg
{
    public string Id { get; set; } = default!;
    public RouteInfo RouteInfo { get; set; } = default!;
    public List<Provider> Providers { get; set; } = default!;
}