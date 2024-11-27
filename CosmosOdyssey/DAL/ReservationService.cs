using Models;
using MongoDB.Driver;

namespace DAL;

public class ReservationService
{
    private readonly MongoDbContext _context;

    public ReservationService(MongoDbContext context)
    {
        _context = context;
    }

    public async Task AddReservation(Reservation reservation)
    {
        await _context.Reservations.InsertOneAsync(reservation);
    }

    public async Task<List<Reservation>> GetReservations()
    {
        return await _context.Reservations.Find(_ => true).ToListAsync();
    }
}