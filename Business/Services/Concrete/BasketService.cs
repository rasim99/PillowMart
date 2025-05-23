using Business.Models.Basket;
using Business.Services.Abstract;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Business.Services.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepistory _basketRepistory;
        private readonly UserManager<User> _userManager;
        private readonly IProductRepistory _productRepistory;
        private readonly IBasketProductRepository _basketProductRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActionContextAccessor _contextAccessor;

        public BasketService(IBasketRepistory basketRepistory,
            UserManager<User> userManager,
            IProductRepistory productRepistory,
            IBasketProductRepository basketProductRepository,
            IUnitOfWork unitOfWork,
            IActionContextAccessor contextAccessor)
        {
            _basketRepistory = basketRepistory;
            _userManager = userManager;
            _productRepistory = productRepistory;
            _basketProductRepository = basketProductRepository;
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task<(int statusCode,string description)> AddProductAsync(int productId)
        {
            var user = _userManager.GetUserAsync(_contextAccessor.ActionContext.HttpContext.User).Result;
            if (user == null) return (401,"səbətə məhsul əlavə etmək olmadı!");
            var product =await _productRepistory.GetAsync(productId);
            if (product == null) return(404, "səbətə məhsul əlavə etmək olmadı!");
            if (product.StockQuantity == 0) return (400,"məhsul  tükənib");

            var basket =await _basketRepistory.GetByUserIdAsync(user.Id);
            if (basket == null)
            {
                basket = new Basket
                {
                    UserId = user.Id,
                    CreatedDate = DateTime.Now
                };
               await  _basketRepistory.CreateAsync(basket);
            }
            var basketProduct = await _basketProductRepository.GetByProductIdAndUserId(product.Id,user.Id);
            if (basketProduct is null)
            {
                basketProduct = new BasketProduct
                {
                    Basket = basket,
                    ProductId = productId,
                    Quantity = 1,
                    CreatedDate = DateTime.Now

                };
               await _basketProductRepository.CreateAsync(basketProduct);

            }
            else
            {
                if (basketProduct.Quantity == product.StockQuantity) return (400,"maksimal məhsul əlavə edilib");

                basketProduct.Quantity ++;
                _basketProductRepository.Update(basketProduct);

            }

            await _unitOfWork.CommitAsync();
            return (200,"məhsul əlavə edildi");
        }

       
        public async Task<BasketIndexVM> GetBasketProducts(string userId)
        {

            var user = _userManager.Users.Include(u => u.Basket).FirstOrDefault(u => u.Id == userId);
            if (user.Basket is null) return new BasketIndexVM();
            var basketProducts = _basketProductRepository.GetBasketProductsWithUserBasket(user);

            foreach (var basketProduct in basketProducts)
            {
                if (basketProduct.Quantity > basketProduct.Product.StockQuantity)
                {
                    basketProduct.Quantity = basketProduct.Product.StockQuantity;
                    basketProduct.HasChanged = true;
                        _basketProductRepository.Update(basketProduct);
                }
            }
           await _unitOfWork.CommitAsync();


            var model = new BasketIndexVM
            {
                BasketProducts = basketProducts

            };

            return model;

        }

        public BasketProduct GetBasketProductById(int basketProductId)
        {
            return _basketProductRepository.GetBasketProductById(basketProductId);
        }
       
       
        public async Task DeleteBasketProduct(BasketProduct basketProduct)
        {
            _basketProductRepository.Delete(basketProduct);
            await _unitOfWork.CommitAsync();
        }

        public decimal CalculateTotalPrice(int basketId)
        {
            return _basketProductRepository.CalculateTotalPrice(basketId);
        }
    }

}

