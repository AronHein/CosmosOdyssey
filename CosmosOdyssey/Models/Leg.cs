namespace Models;

public class Leg
{
    public Guid Id { get; set; }
    public RouteInfo RouteInfo { get; set; }
    public List<Provider> Providers { get; set; }
}