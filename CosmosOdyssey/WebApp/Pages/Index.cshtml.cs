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
    
    [BindProperty]
    public string From { get; set; }

    [BindProperty]
    public string To { get; set; }
    [BindProperty]
    public List<string> Locations { get; set; }

    public async Task OnGet()
    {
        var pricelist = await _pricelistService.GetLatestPricelist();
        Locations = pricelist.Legs.Select(l => l.RouteInfo.From.Name).Distinct().ToList();
    }
    public void OnPost()
    {
        _logger.LogInformation($"From: {From}, To: {To}");
    }
}