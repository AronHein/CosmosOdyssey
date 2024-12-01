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
    
    public async Task FetchAndAddPricelistIfPriceListNew()
    {
        var json = await FetchPricelistJson();
        var pricelist = DeserializePricelistJson(json);
        var latestPricelist = await GetLatestPricelist();
        
        if (pricelist.Id != latestPricelist.Id)
        {
            await AddPricelist(pricelist);
        }
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
        await DeleteOldestPricelistIfOverLimit(15);
        
        await DeleteDatedReservations();

        await _context.Pricelists.InsertOneAsync(pricelist);
    }

    public async Task<List<Pricelist>> GetPricelists()
    {
        return await _context.Pricelists.Find(_ => true).ToListAsync();
    }
    
    // Depth-first search algorithm to find all possible routes from one location to another in pricelist.
    public List<List<Leg>> FindAllRoutes(Pricelist pricelist, string from, string to)
    {
        var routes = new List<List<Leg>>();
        var legs = pricelist.Legs;
        
        // Helper method to perform DFS.
        void Search(List<Leg> currentRoute, string currentLocation)
        {
            if (currentLocation == to)
            {
                if (currentRoute.Count > 0)
                {
                    routes.Add(new List<Leg>(currentRoute));
                }
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

    public async Task<Pricelist> GetLatestPricelist()
    {
        return await _context.Pricelists
            .Find(_ => true)
            .SortByDescending(p => p.ValidUntil)
            .FirstOrDefaultAsync();
    }
    
    public List<Provider> FindProviders(List<string> providerIds)
    {
        List<Provider> selectedProviders = new List<Provider>();

        foreach (var providerId in providerIds)
        {
            foreach (var pricelist in _context.Pricelists.AsQueryable())
            {
                foreach (var leg in pricelist.Legs)
                {
                    var provider = leg.Providers.FirstOrDefault(p => p.Id == providerId);
                    if (provider != null && !selectedProviders.Contains(provider))
                    {
                        selectedProviders.Add(provider);
                    }
                }
            }
        }

        return selectedProviders;
    }
    public List<Leg> FindLegsFromProviderIds(List<string> providerIds)
    {
        List<Leg> selectedLegs = new List<Leg>();

        foreach (var providerId in providerIds)
        {
            foreach (var pricelist in _context.Pricelists.AsQueryable())
            {
                foreach (var leg in pricelist.Legs)
                {
                    if (leg.Providers.Any(provider => provider.Id == providerId) &&
                        !selectedLegs.Contains(leg))
                    {
                        selectedLegs.Add(leg);
                    }
                }
            }
        }
        return selectedLegs;
    }
    
    private async Task DeleteOldestPricelistIfOverLimit(int limit)
    {
        var count = await _context.Pricelists.CountDocumentsAsync(_ => true);

        if (count >= limit)
        {
            var oldestPricelist = await _context.Pricelists
                .Find(_ => true)
                .SortBy(p => p.ValidUntil)
                .FirstOrDefaultAsync();

            if (oldestPricelist != null)
            {
                await _context.Pricelists.DeleteOneAsync(p => p.Id == oldestPricelist.Id);
            }
        }
    }
    
    private async Task DeleteDatedReservations()
    {
        var reservations = await _context.Reservations.Find(_ => true).ToListAsync();
        var pricelists = await GetPricelists();
        var pricelistIds = pricelists.Select(p => p.Id).ToList();
        
        foreach (var reservation in reservations)
        {
            if (!pricelistIds.Contains(reservation.PricelistId))
            {
                await _context.Reservations.DeleteOneAsync(r => r.Id == reservation.Id);
            }
        }
    }
}