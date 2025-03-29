

using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Repositories.Concrete
{
    public class CategoryRepistory : BaseRepistory<Category>, ICategoryRepistory
    {
        private readonly AppDbContext _context;

        public CategoryRepistory(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async  Task<Category> GetByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c=>c.Name.Trim().ToLower()==name.Trim().ToLower());
        }

        public List<SelectListItem> GetSelectListItems()
        {
          
            return _context.Categories.Select(c=>new SelectListItem
            {
                Text = c.Name,
                Value=c.Id.ToString()
            }).ToList();
        }
    }
}
