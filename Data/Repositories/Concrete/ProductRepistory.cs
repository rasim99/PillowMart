
using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Data.Repositories.Concrete
{
    public class ProductRepistory : BaseRepistory<Product>, IProductRepistory
    {
        private readonly AppDbContext _context;

        public ProductRepistory(AppDbContext context) : base(context)
        {
           _context = context;
        }


        public IQueryable<Product> GetProductsWithCategory()
        {
            return   _context.Products.Include(p => p.Category);
        } 


    }
}
