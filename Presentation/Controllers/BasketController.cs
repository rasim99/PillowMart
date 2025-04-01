using Business.Models.Basket;
using Business.Services.Abstract;
using Business.Services.Concrete;
using Core.Entities;
using Core.Utilities.Stripe;
using Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Presentation.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly StripeSettings _stripeSettings;

        public BasketController(IBasketService basketService,UserManager<User>userManager,AppDbContext context,
            IOptions<StripeSettings> stripeSettings)
        {
            _basketService = basketService;
            _userManager = userManager;
            _context = context;
            _stripeSettings = stripeSettings.Value;
        }

        public async  Task<IActionResult> Index()
        {
            var authUser = await _userManager.GetUserAsync(User);
            if (authUser == null) return Unauthorized();
            var model =await _basketService.GetBasketProducts(authUser.Id);
            ViewBag.PublishableKey = _stripeSettings.PublishableKey;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(int productId)
        {
            var result = await _basketService.AddProductAsync(productId);
            switch (result.statusCode)
            {
                case 200: return Ok(result.description);
                case 400: return BadRequest(result.description);
                case 401: return Unauthorized(result.description);
                case 404: return NotFound(result.description);
                default: return NotFound();
            }
        }

        [HttpPost]
        public IActionResult IncreaseCount(int basketProductId)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null) return Unauthorized();

            var basketProduct = _context.BasketProducts.Include(bp => bp.Basket).FirstOrDefault(bp => bp.Id == basketProductId);
            if (basketProduct is null) return NotFound("Məhsul səbətdə tapılmadı");

            if (basketProduct.Basket.UserId != user.Id) return BadRequest("məhsulun sayını artırmaq mümkün olmadı");


            var product = _context.Products.Find(basketProduct.ProductId);
            if (product is null) return NotFound("məhsul tapılmadı");

            if (basketProduct.Quantity == product.StockQuantity) return BadRequest(" maksimum məhsul əlavə edildi");

            basketProduct.Quantity++;
            _context.BasketProducts.Update(basketProduct);
            _context.SaveChanges();
            return Ok(new
            {
                quantity = basketProduct.Quantity,
                productTotalPrice = basketProduct.Quantity * product.Price,
                totalPrice = _context.BasketProducts.Include(bp => bp.Product)
                                .Where(bp => bp.BasketId == user.Basket.Id)
                                .Sum(bp => bp.Quantity * bp.Product.Price)
            });
        }

        [HttpPost]
        public IActionResult DecreaseCount(int basketProductId)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null) return Unauthorized();
            var basketProduct = _context.BasketProducts.Include(bp => bp.Basket).FirstOrDefault(bp => bp.Id == basketProductId);
            if (basketProduct is null) return NotFound("Məhsul səbətdə tapılmadı");

            if (basketProduct.Basket.UserId != user.Id) return BadRequest("cməhsulun sayını azaltmaq mümkün olmadı  ");


            var product = _context.Products.Find(basketProduct.ProductId);
            if (product is null) return NotFound(" məhsul tapılmadı");

            if (basketProduct.Quantity == 1) return BadRequest(" məhsul sayı minimum 1 olmalıdır");

            basketProduct.Quantity--;
            _context.BasketProducts.Update(basketProduct);

            _context.SaveChanges();
            return Ok(new
            {
                quantity = basketProduct.Quantity,
                productTotalPrice = basketProduct.Quantity * product.Price,
                totalPrice = _context.BasketProducts.Include(bp => bp.Product)
                                   .Where(bp => bp.BasketId == user.Basket.Id)
                                   .Sum(bp => bp.Quantity * bp.Product.Price)
            });

        }
     
        [HttpPost]
        public async  Task<IActionResult> DeleteProduct(int basketProductId)
        {
            var user =await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var basketProduct = _basketService.GetBasketProductById(basketProductId);
            if (basketProduct == null) return NotFound("  səbətdə məhsul tapılmadı");

            if (basketProduct.Basket.UserId != user.Id)
                return BadRequest("məhsul silinə bilinmədi");

           await  _basketService.DeleteBasketProduct(basketProduct);

            var totalPrice = _basketService.CalculateTotalPrice(basketProduct.BasketId);
            return Ok(new { totalPrice });
        }

    }
}
