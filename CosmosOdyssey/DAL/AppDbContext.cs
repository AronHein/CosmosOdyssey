using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Pricelist> Pricelists { get; set; }
    public DbSet<Leg> Legs { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<RouteInfo> RouteInfos { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Location> Locations { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}