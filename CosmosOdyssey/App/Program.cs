using var client = new HttpClient();
const string apiUrl = "https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices";

var response = await client.GetAsync(apiUrl);
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine(content);