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

            if (string.IsNullOrEmpty(SortCriteria))
            {
                SortCriteria = "time";
            }
            SortRoutes();
        }

        public async Task<IActionResult> OnPost()
        {
            await UpdatePricelistAndRoutes();
            
            if (string.IsNullOrEmpty(FirstName))
            {
                ErrorMessage = "Please enter your first name.";
                return Page();
            }
            if (string.IsNullOrEmpty(LastName))
            {
                ErrorMessage = "Please enter your last name.";
                return Page();
            }
            
            
            List<Provider> selectedProviders = _pricelistService.FindProviders(ProviderIds);
            List<Leg> selectedLegs = _pricelistService.FindLegsFromProviderIds(ProviderIds);
            
            if (!IsValidRoute(selectedLegs))
            {
                ErrorMessage = "Invalid selection: Please select exactly one flight for each leg of a single route.";
                return Page();
            }

            if (IsFlightTimesOverlapping(selectedProviders))
            {
                ErrorMessage = "Invalid selection: Flight times are overlapping.";
                return Page();
            }
            
            var reservation = new Reservation
            {
                PricelistId = Pricelist.Id,
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
        
        private bool IsValidRoute(List<Leg> selectedLegs)
        {
            var selectedRoute = selectedLegs.Select(leg => leg.RouteInfo).ToList();

            foreach (var route in Routes)
            {
                if (RouteMatches(route, selectedRoute))
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool RouteMatches(List<Leg> route, List<RouteInfo> selectedRoute)
        {
            if (route.Count != selectedRoute.Count)
            {
                return false;
            }

            for (int i = 0; i < route.Count; i++)
            {
                if (route[i].RouteInfo.From.Name != selectedRoute[i].From.Name ||
                    route[i].RouteInfo.To.Name != selectedRoute[i].To.Name)
                {
                    return false;
                }
            }

            return true;
        }
        private bool IsFlightTimesOverlapping(List<Provider> selectedProviders)
        {
            var selectedProvidersCount = selectedProviders.Count;
            
            for (int i = 0; i < selectedProvidersCount; i++)
            {
                for (int j = i + 1; j < selectedProvidersCount; j++)
                {
                    if (selectedProviders[i].FlightStart < selectedProviders[j].FlightEnd &&
                        selectedProviders[j].FlightStart < selectedProviders[i].FlightEnd)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}