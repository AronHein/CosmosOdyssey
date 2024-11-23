using DAL;

var class1 = new TravelPriceFetcher();
var travelPrices = await class1.GetTravelPrices();

Console.WriteLine(travelPrices.Legs[0].Providers[0].Price);
Console.WriteLine(travelPrices.ValidUntil);

