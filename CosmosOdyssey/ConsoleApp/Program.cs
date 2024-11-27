using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddHttpClient();

// TODO: add filepath helper
var filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../../../"));
var builder = new ConfigurationBuilder()
    .SetBasePath(filePath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = builder.Build();

var connectionString = configuration.GetConnectionString("MongoDb");
var databaseName = configuration["DatabaseName"];
Console.WriteLine(connectionString);
Console.WriteLine(databaseName);

serviceCollection.AddSingleton(new MongoDbContext(connectionString, databaseName));

serviceCollection.AddTransient<PricelistService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var pricelistService = serviceProvider.GetService<PricelistService>();

var json = await pricelistService.FetchPricelistJson();
var pricelist = pricelistService.DeserializePricelistJson(json);
Console.WriteLine(pricelist.Legs[0].Providers[0].Price);
await pricelistService.AddPricelistAsync(pricelist);
var pricelists = await pricelistService.GetPricelistsAsync();
Console.WriteLine(pricelists.First().Legs.First().Providers.First().Price);