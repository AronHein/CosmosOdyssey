using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=cosmosOdyssey.db"));
serviceCollection.AddHttpClient<TravelPriceFetcher>();

var serviceProvider = serviceCollection.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var fetcher = scope.ServiceProvider.GetRequiredService<TravelPriceFetcher>();

    var pricelist = await fetcher.GetPricelist();

    Console.WriteLine(pricelist.Legs[0].Providers[0].Price);
}

