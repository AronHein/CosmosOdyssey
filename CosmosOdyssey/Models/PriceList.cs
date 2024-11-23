namespace Models;

public class Pricelist
{
    public Guid Id { get; set; }
    public DateTime ValidUntil { get; set; }
    public List<Leg> Legs { get; set; }
}