using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<PricelistJson> PricelistJson { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}