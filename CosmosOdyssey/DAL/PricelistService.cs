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
    
    public async Task AddPricelist(Pricelist pricelist)
    {
        await _context.Pricelists.InsertOneAsync(pricelist);
    }

    public async Task<List<Pricelist>> GetPricelists()
    {
        return await _context.Pricelists.Find(_ => true).ToListAsync();
    }
    
    // Depth-first search algorithm to find all possible routes from one location to another.
    public List<List<Leg>> FindAllRoutes(Pricelist pricelist, string from, string to)
    {
        var routes = new List<List<Leg>>();
        var legs = pricelist.Legs;

        void Search(List<Leg> currentRoute, string currentLocation)
        {
            if (currentLocation == to)
            {
                routes.Add(new List<Leg>(currentRoute));
                return;
            }
            
            foreach (var leg in legs)
            {
                
                if (leg.RouteInfo.From.Name == currentLocation && 
                    // Check if the leg hasn't already been used in the route.
                    !currentRoute.Any(l => l.RouteInfo.From.Name == leg.RouteInfo.From.Name))
                {
                    currentRoute.Add(leg);
                    Search(currentRoute, leg.RouteInfo.To.Name);
                    currentRoute.RemoveAt(currentRoute.Count - 1); // Backtrack
                }
            }
        }
        
        Search(new List<Leg>(), from);

        return routes;
    }
    
    public List<Dictionary<string, string>> GetRouteNamesFromRoutesList(List<List<Leg>> routes)
    {
        var routeNames = new List<Dictionary<string, string>>();
        foreach (var route in routes)
        {
            var routeName = new Dictionary<string, string>();
            foreach (var leg in route)
            {
                routeName.Add(leg.RouteInfo.From.Name, leg.RouteInfo.To.Name);
            }
            routeNames.Add(routeName);
        }
        return routeNames;
    }

    public async Task<Pricelist> GetLatestPricelist()
    {
        return await _context.Pricelists
            .Find(_ => true)
            .SortByDescending(p => p.ValidUntil)
            .FirstOrDefaultAsync();
    }
}