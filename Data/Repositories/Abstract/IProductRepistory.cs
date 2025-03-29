
using Core.Entities;
using Data.Repositories.Base;

namespace Data.Repositories.Abstract
{
    public interface IProductRepistory :IBaseRepistory<Product>
    {
        IQueryable<Product> GetProductsWithCategory();
    }
}
