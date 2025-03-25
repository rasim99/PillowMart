using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
