using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class SubscribeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
