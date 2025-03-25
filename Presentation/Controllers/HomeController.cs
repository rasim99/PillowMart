using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

    }
}
