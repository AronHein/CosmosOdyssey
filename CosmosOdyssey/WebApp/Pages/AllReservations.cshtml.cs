using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using DAL;

namespace WebApp.Pages;

public class AllReservationsModel : PageModel
{
    private readonly ReservationService _reservationService;

    public AllReservationsModel(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    public List<Reservation> Reservations { get; set; } = default!;

    public async Task OnGet()
    {
        Reservations = await _reservationService.GetReservations();
    }
}