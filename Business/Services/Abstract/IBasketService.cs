
using Business.Models.Basket;
using Core.Entities;

namespace Business.Services.Abstract
{
    public interface IBasketService
    {
        Task<(int statusCode,string description)> AddProductAsync(int productId);
        Task<BasketIndexVM> GetBasketProducts(string userId);
        BasketProduct GetBasketProductById(int basketProductId);
        Task DeleteBasketProduct(BasketProduct basketProduct);
        decimal CalculateTotalPrice(int basketId);
    }
}
