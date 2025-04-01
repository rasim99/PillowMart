
using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Concrete
{
    public class BasketProductRepository : BaseRepistory<BasketProduct>, IBasketProductRepository
    {
        private readonly AppDbContext _context;

        public BasketProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public BasketProduct GetBasketProductById(int basketProductId)
        {
            return _context.BasketProducts
                .Include(bp => bp.Basket)
                .FirstOrDefault(bp => bp.Id == basketProductId);
        }

        public async Task<BasketProduct> GetByProductIdAndUserId(int productId, string userId)
        {
            return await _context.BasketProducts.FirstOrDefaultAsync(bp=>bp.ProductId==productId && bp.Basket.UserId==userId);
        }
        public async Task<BasketProduct> GetBasketProductsWithBaskeAsync(int basketProductId)
        {
           return await  _context.BasketProducts.Include(bp => bp.Basket).FirstOrDefaultAsync(bp => bp.Id == basketProductId);
        }

        public List<BasketProduct> GetBasketProductsWithUserBasket(User user)
        {
            return _context.BasketProducts.Include(bp => bp.Product).ThenInclude(p => p.Category).Where(bp => bp.BasketId == user.Basket.Id).ToList();
        }
        public decimal CalculateTotalPrice(int basketId)
        {
            return _context.BasketProducts
                .Include(bp => bp.Product)
                .Where(bp => bp.BasketId == basketId)
                .Sum(bp => bp.Quantity * bp.Product.Price);
        }

    }
}
