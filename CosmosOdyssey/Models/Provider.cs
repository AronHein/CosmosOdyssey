namespace Models;

public class Provider
{
    public string Id { get; set; } = default!;
    public Company Company { get; set; } = default!;
    public double Price { get; set; }
    public DateTime FlightStart { get; set; }
    public DateTime FlightEnd { get; set; }
}