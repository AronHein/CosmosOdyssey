namespace Models;

public class Pricelist
{
    public string Id { get; set; }
    public DateTime ValidUntil { get; set; }
    public List<Leg> Legs { get; set; }
}