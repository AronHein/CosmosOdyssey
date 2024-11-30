namespace Models;

public class Reservation
{
    public string Id { get; set; } = default!;

    public string PricelistId { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public List<RouteInfo> Routes { get; set; } = default!;
    public double TotalQuotedPrice { get; set; }
    public TimeSpan TotalQuotedTravelTime { get; set; }
    public List<string> Companies { get; set; } = default!;
}