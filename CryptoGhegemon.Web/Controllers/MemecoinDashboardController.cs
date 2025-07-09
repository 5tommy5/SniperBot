using CryptoGhegemon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoGhegemon.Web.Controllers;

public class MemecoinDashboardController : Controller
{
    private readonly ILogger<MemecoinDashboardController> _logger;
    private readonly MemecoinCache _memecoinCache;

    public MemecoinDashboardController(ILogger<MemecoinDashboardController> logger, MemecoinCache cache)
    {
        _logger = logger;
        _memecoinCache = cache;
    }

    public IActionResult Index()
    {
        ViewBag.Memecoins = _memecoinCache.Get(50);
        return View();
    }
}