using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Teatar18_2.Data;
using Teatar18_2.Models;
using Teatar18_2.Services;

namespace Teatar18_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly PredstavaService _predstavaService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, PredstavaService predstavaService)
        {
            _logger = logger;
            _context = context;
            _predstavaService = predstavaService;
        }


        public async Task<IActionResult> Index()
        {
            var predstave = await _predstavaService.DajPreporuke();
            return View(predstave);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DajInformacijuOUstanovi()
        {
            return View("ONamaView");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
