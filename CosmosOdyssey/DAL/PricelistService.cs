namespace DAL;

public class PricelistService
{
    private readonly HttpClient _client;
    private const string ApiUrl = "https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices";

    public PricelistService(HttpClient client)
    {
        _client = client;
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
}