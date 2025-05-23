using Business.Models.Admin.Product;
using Business.Services.Abstract;
using Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("00110110")]
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        private readonly IProductService _productService;
       
        public ProductController(IProductService productService )
        {
            _productService = productService;
        }

        #region List
        public  IActionResult Index(ProductFilterVM model)
        {
            ViewBag.isProductController = true;
            ViewBag.isIndexAction = true;

            model.Products = _productService.FilteredProduct(model);
        
            return View(model);
        } 

        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = _productService.Create();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create( ProductCreateVM model)
        {
            var isSucceeded=await _productService.CreateAsync(model);
            if(!isSucceeded) return View(model);
            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Update 

        [HttpGet]
        public async  Task<IActionResult> Update(int id)
        {
            var model = await _productService.UpdateAsync(id);
            if(model is null) return NotFound();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult>Update(int id, ProductUpdateVM model)
        {
            var isSucceeded= await _productService.UpdateAsync(id, model);
            if(!isSucceeded) return View(model);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
           var isSucceeded=await _productService.DeleteAsync(id);
            if(!isSucceeded) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
