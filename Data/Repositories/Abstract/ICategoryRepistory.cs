using Core.Entities;
using Data.Repositories.Base;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Data.Repositories.Abstract
{
    public interface ICategoryRepistory :IBaseRepistory<Category>
    {
        Task<Category> GetByNameAsync(string name);
        List<SelectListItem> GetSelectListItems();
    }
}
