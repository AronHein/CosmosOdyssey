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
        
        [BindProperty(SupportsGet = true)]
        public string SortCriteria { get; set; }

        public async Task OnGet()
        {
            if (!string.IsNullOrEmpty(From) && !string.IsNullOrEmpty(To))
            { 
                Pricelist = await _pricelistService.GetLatestPricelist();
                Routes = _pricelistService.FindAllRoutes(Pricelist, From, To);

                if (!string.IsNullOrEmpty(SortCriteria))
                {
                    SortRoutes();
                }
            }
        }

        private void SortRoutes()
        {
            if (SortCriteria == "distance")
            {
                Routes = Routes.OrderBy(r => r.Sum(leg => leg.RouteInfo.Distance)).ToList();
            }
                
            foreach (var route in Routes)
            {
                foreach (var leg in route)
                {
                    if (SortCriteria == "price")
                    {
                        leg.Providers.Sort((provider1, provider2) =>
                            provider1.Price.CompareTo(provider2.Price));
                    }
                    else if (SortCriteria == "time")
                    {
                        leg.Providers.Sort((provider1, provider2) =>
                            provider1.FlightStart.CompareTo(provider2.FlightStart));
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("AllReservations");
        }
    }
}