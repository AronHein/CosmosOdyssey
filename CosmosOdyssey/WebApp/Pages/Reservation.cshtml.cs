using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using DAL;

namespace WebApp.Pages
{
    public class ReservationModel : PageModel
    {
        private readonly PricelistService _pricelistService;
        private readonly ReservationService _reservationService;

        public ReservationModel(PricelistService pricelistService, ReservationService reservationService)
        {
            _pricelistService = pricelistService;
            _reservationService = reservationService;
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
        
        [BindProperty]
        public string FirstName { get; set; }
        
        [BindProperty]
        public string LastName { get; set; }
        
        [BindProperty]
        public List<string> ProviderIds { get; set; }
        
        [BindProperty] 
        public string ErrorMessage { get; set; }

        public async Task OnGet()
        {
            await UpdatePricelistAndRoutes();

            if (!string.IsNullOrEmpty(SortCriteria))
            {
                SortRoutes();
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                await UpdatePricelistAndRoutes();
                ErrorMessage = "Please enter your first name.";
                return Page();
            }
            if (string.IsNullOrEmpty(LastName))
            {
                await UpdatePricelistAndRoutes();
                ErrorMessage = "Please enter your last name.";
                return Page();
            }
            
            
            List<Provider> selectedProviders = _pricelistService.FindProviders(ProviderIds);
            List<Leg> selectedLegs = _pricelistService.FindLegsFromProviderIds(ProviderIds);
            
            var reservation = new Reservation
            {
                FirstName = FirstName,
                LastName = LastName,
                Routes = selectedLegs.Select(leg => leg.RouteInfo).ToList(),
                TotalQuotedPrice = selectedProviders.Sum(p => p.Price),
                TotalQuotedTravelTime = TimeSpan.FromMinutes(selectedProviders.Sum(p => (p.FlightEnd - p.FlightStart).TotalMinutes)),
                Companies = selectedProviders.Select(p => p.Company.Name).ToList()
            };
            
            await _reservationService.AddReservation(reservation);
            return RedirectToPage("AllReservations");
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
        
        private async Task UpdatePricelistAndRoutes()
        {
            await _pricelistService.FetchAndAddPricelistIfPriceListNew();
            
            Pricelist = await _pricelistService.GetLatestPricelist();
            Routes = _pricelistService.FindAllRoutes(Pricelist, From, To);
        }
    }
}