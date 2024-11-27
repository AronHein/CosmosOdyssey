using Models;
using MongoDB.Driver;

namespace DAL;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Pricelist> Pricelists => _database.GetCollection<Pricelist>("Pricelists");
    public IMongoCollection<Reservation> Reservations => _database.GetCollection<Reservation>("Reservations");
}