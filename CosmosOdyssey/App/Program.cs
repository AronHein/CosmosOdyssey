using System.Text.Json;
using Models;

using var client = new HttpClient();
const string apiUrl = "https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices";

var response = await client.GetAsync(apiUrl);
var content = await response.Content.ReadAsStringAsync();
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};
var travelPrices = JsonSerializer.Deserialize<Pricelist>(content, options);

Console.WriteLine(travelPrices.Legs[0].Providers[0].Price);
Console.WriteLine(travelPrices.ValidUntil);