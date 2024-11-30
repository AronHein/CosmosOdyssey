namespace Models;

public class Pricelist
{
    public string Id { get; set; } = default!;
    public DateTime ValidUntil { get; set; } = default!;
    public List<Leg> Legs { get; set; } = default!;
}