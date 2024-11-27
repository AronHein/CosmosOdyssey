using System.Text.Json;
using Models;
using MongoDB.Driver;

namespace DAL;

public class PricelistService
{
    private readonly HttpClient _client;
    private readonly MongoDbContext _context;
    private const string ApiUrl = "https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices";

    public PricelistService(HttpClient client, MongoDbContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task<string> FetchPricelistJson()
    {
        try
        {
            var response = await _client.GetAsync(ApiUrl);
            var content = await response.Content.ReadAsStringAsync();
            return content;

        }
        catch (Exception e)
        {
            throw new Exception("Error occurred while trying to fetch data from the API: " + e);
        }
    }
    
    public Pricelist DeserializePricelistJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var pricelist = JsonSerializer.Deserialize<Pricelist>(json, options);
        
        if (pricelist == null)
        {
            throw new JsonException("Failed to deserialize the API response into a pricelist object.");
        }
        return pricelist;
    }
    
    public async Task AddPricelistAsync(Pricelist pricelist)
    {
        await _context.Pricelists.InsertOneAsync(pricelist);
    }

    public async Task<List<Pricelist>> GetPricelistsAsync()
    {
        return await _context.Pricelists.Find(_ => true).ToListAsync();
    }
}