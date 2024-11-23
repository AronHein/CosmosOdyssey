namespace Models;

public class Provider
{
    public Guid Id { get; set; }
    public Company Company { get; set; }
    public decimal Price { get; set; }
    public DateTime FlightStart { get; set; }
    public DateTime FlightEnd { get; set; }
}