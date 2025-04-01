using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Concrete
{
    public class BasketRepistory : BaseRepistory<Basket>, IBasketRepistory
    {
        private readonly AppDbContext _context;

        public BasketRepistory(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Basket> GetByUserIdAsync(string userId)
        {
            return await _context.Baskets.FirstOrDefaultAsync(b => b.UserId == userId);
        }

        public Basket GetBasketWithProductsByUserId(string userId)
        {
                        var basket = _context.Baskets
                            .Include(b => b.BasketProducts)
                            .ThenInclude(bp => bp.Product)
                            .FirstOrDefault(b => b.UserId == userId);

            if (basket is null || basket.BasketProducts is null) return null;
                return basket;
        }
        public void UpdateBasketProducts(List<BasketProduct> basketProducts)
        {
            _context.BasketProducts.UpdateRange(basketProducts);
        }

     
    }
}
