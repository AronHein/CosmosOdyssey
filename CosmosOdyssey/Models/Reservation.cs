namespace Models;

public class Reservation
{
    public string Id { get; set; }

    public string PricelistId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<RouteInfo> Routes { get; set; }
    public double TotalQuotedPrice { get; set; }
    public TimeSpan TotalQuotedTravelTime { get; set; }
    public List<string> Companies { get; set; }
}