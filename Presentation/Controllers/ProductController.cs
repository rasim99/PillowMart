using Business.Models.Product;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService )
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var  model =await  _productService.GetProductsAsync();
            return View(model.OrderByDescending(p => p.CreatedDate).ToList());
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
