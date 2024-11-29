using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using DAL;

namespace WebApp.Pages
{
    public class ReservationModel : PageModel
    {
        private readonly PricelistService _pricelistService;

        public ReservationModel(PricelistService pricelistService)
        {
            _pricelistService = pricelistService;
        }

        [BindProperty(SupportsGet = true)]
        public string From { get; set; }

        [BindProperty(SupportsGet = true)]
        public string To { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public Pricelist Pricelist { get; set; }
        
        [BindProperty]
        public List<List<Leg>> Routes { get; set; }

        [BindProperty]
        public string SelectedFlight { get; set; }

        public async Task OnGet()
        {
            if (!string.IsNullOrEmpty(From) && !string.IsNullOrEmpty(To))
            { 
                Pricelist = await _pricelistService.GetLatestPricelist();
                Routes = _pricelistService.FindAllRoutes(Pricelist, From, To);
            }
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("AllReservations");
        }
    }
}