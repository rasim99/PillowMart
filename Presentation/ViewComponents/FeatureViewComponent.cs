using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class FeatureViewComponent :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
