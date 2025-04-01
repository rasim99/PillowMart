using Core.Entities;
using Data.Repositories.Base;


namespace Data.Repositories.Abstract
{
    public interface IBasketRepistory : IBaseRepistory<Basket>
    {
        Task<Basket> GetByUserIdAsync(string userId);
        Basket GetBasketWithProductsByUserId(string userId);
        void UpdateBasketProducts(List<BasketProduct> basketProducts);
    }
}
