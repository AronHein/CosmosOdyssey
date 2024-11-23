namespace Models;

public class PricelistJson
{
    public Guid Id { get; set; }
    public DateTime ValidUntil { get; set; }
    public string Json { get; set; }
}