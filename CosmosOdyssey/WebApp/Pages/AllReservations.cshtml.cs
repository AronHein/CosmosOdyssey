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
    
    public static string FormatTimeSpan(TimeSpan timeSpan)
    {
        string formattedTime = "";

        if (timeSpan.Days > 0)
            formattedTime += $"{timeSpan.Days} day{(timeSpan.Days > 1 ? "s" : "")}";

        if (timeSpan.Hours > 0)
        {
            if (formattedTime.Length > 0)
                formattedTime += ", ";

            formattedTime += $"{timeSpan.Hours} hour{(timeSpan.Hours > 1 ? "s" : "")}";
        }

        if (timeSpan.Minutes > 0)
        {
            if (formattedTime.Length > 0)
                formattedTime += ", ";

            formattedTime += $"{timeSpan.Minutes} minute{(timeSpan.Minutes > 1 ? "s" : "")}";
        }

        return formattedTime;
    }
    
    public static string FormatRoute(List<RouteInfo> routes)
    {
        var fullRoute = routes.First().From.Name;
        foreach (var routeInfo in routes)
        {
            fullRoute += " -> " + routeInfo.To.Name;
        }
        return fullRoute;
    }
}