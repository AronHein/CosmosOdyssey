using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;

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
serviceCollection.AddTransient<ReservationService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var pricelistService = serviceProvider.GetService<PricelistService>();

var json = await pricelistService.FetchPricelistJson();
var pricelist = pricelistService.DeserializePricelistJson(json);
// Console.WriteLine(pricelist.Legs[0].Providers[0].Price);
// await pricelistService.AddPricelist(pricelist);
// var pricelists = await pricelistService.GetPricelists();
// Console.WriteLine(pricelists.First().Legs.First().Providers.First().Price);
// var reservation = new Reservation
// {
//     Id = Guid.NewGuid().ToString(),
//     FirstName = "mari",
//     LastName = "tamm",
//     Routes = new List<RouteInfo>
//     {
//         new RouteInfo
//         {
//             Id = Guid.NewGuid().ToString(),
//             From = new Location { Name = "Earth" },
//             To = new Location { Name = "Mars" },
//             Distance = 225000000
//         }
//     },
//     TotalQuotedPrice = 123.45,
//     TotalQuotedTravelTime = new TimeSpan(5, 30, 0),
//     Companies = new List<string> { "CompanyA", "CompanyB" }
// };
//
// var reservationService = serviceProvider.GetService<ReservationService>();
// await reservationService.AddReservation(reservation);
// var reservations = await reservationService.GetReservations();
// Console.WriteLine(reservations.First().FirstName);

var routes = pricelistService.FindAllRoutes(pricelist, "Uranus", "Earth");
Console.WriteLine(routes.Count);
var routeNames = pricelistService.GetRouteNamesFromRoutesList(routes);
foreach (var route in routeNames)
{
    foreach (var kvp in route)
    {
        Console.WriteLine($"from: {kvp.Key} to: {kvp.Value}");
    }

    Console.WriteLine("---------------------");
}

