using System.Text.Json;
using Models;

namespace DAL;


public class TravelPriceFetcher
{
    private readonly HttpClient _client = new HttpClient();
    private const string ApiUrl = "https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices";

    public async Task<Pricelist> GetTravelPrices()
    {
        try
        {
            var response = await _client.GetAsync(ApiUrl);
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var pricelist = JsonSerializer.Deserialize<Pricelist>(content, options);
            
            if (pricelist == null)
            {
                throw new JsonException("Failed to deserialize the API response into a pricelist object.");
            }
            return pricelist;

        }
        catch (HttpRequestException e)
        {
            throw new Exception("Error occurred while trying to fetch data from the API: " + e);
        }
        catch (JsonException e)
        {
            throw new Exception("Error occurred while deserializing the API response: " + e);
        }
        catch (Exception e)
        {
            throw new Exception("An unexpected error occurred while fetching pricelist: " + e);
        }
    }
}