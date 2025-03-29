using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas._00110110.Controllers
{
    [Area("00110110")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
