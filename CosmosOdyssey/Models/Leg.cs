namespace Models;

public class Leg
{
    public string Id { get; set; }
    public RouteInfo RouteInfo { get; set; }
    public List<Provider> Providers { get; set; }
}