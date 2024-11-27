using DAL;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddHttpClient();
serviceCollection.AddTransient<PricelistService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var pricelistService = serviceProvider.GetService<PricelistService>();

var json = await pricelistService.FetchPricelistJson();
var pricelist = pricelistService.DeserializePricelistJson(json);
Console.WriteLine(pricelist.Legs[0].Providers[0].Price);