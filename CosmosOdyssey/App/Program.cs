using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=../app.db"));
serviceCollection.AddHttpClient<TravelPriceFetcher>();

var serviceProvider = serviceCollection.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var fetcher = scope.ServiceProvider.GetRequiredService<TravelPriceFetcher>();
    
    context.Database.Migrate();
    
    var pricelist = await fetcher.GetPricelist();
    Console.WriteLine(pricelist.Legs[0].Providers[0].Price);
    await fetcher.SavePricelistToDb(pricelist);
    
    var savedPricelist = context.PricelistJson
        .FirstOrDefault(p => p.ValidUntil == pricelist.ValidUntil);
    Console.WriteLine("Saved " + savedPricelist.Id);
}