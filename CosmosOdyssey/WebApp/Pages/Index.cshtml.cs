using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly PricelistService _pricelistService;

    public IndexModel(ILogger<IndexModel> logger, PricelistService pricelistService)
    {
        _logger = logger;
        _pricelistService = pricelistService;
    }

    [BindProperty] public string From { get; set; } = default!;

    [BindProperty] public string To { get; set; } = default!;
    [BindProperty] public List<string> Locations { get; set; } = default!;
    [BindProperty] public string ErrorMessage { get; set; } = default!;

    public async Task<IActionResult> OnPost()
    {
        await PopulateLocations();
        
        if (!string.IsNullOrEmpty(From) && !string.IsNullOrEmpty(To) && From == To)
        {
            ErrorMessage = "The 'From' and 'To' locations cannot be the same.";
            return Page();

        }
        
        return RedirectToPage("Reservation", new { From, To });
    }
    
    public async Task OnGet()
    {
        await PopulateLocations();
    }

    private async Task PopulateLocations()
    {
        var pricelist = await _pricelistService.GetLatestPricelist();
        Locations = pricelist.Legs.Select(l => l.RouteInfo.From.Name).Distinct().ToList();
    }
}