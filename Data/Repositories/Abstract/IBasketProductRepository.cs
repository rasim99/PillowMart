using Core.Entities;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Abstract
{
    public interface IBasketProductRepository:IBaseRepistory<BasketProduct>
    {
        Task<BasketProduct> GetByProductIdAndUserId(int productId,string userId);

        List<BasketProduct> GetBasketProductsWithUserBasket(User user);

        BasketProduct GetBasketProductById(int basketProductId);
        Task<BasketProduct> GetBasketProductsWithBaskeAsync(int basketProductId);
        decimal CalculateTotalPrice(int basketId);

    }
}
