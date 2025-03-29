

using Business.Models.Admin.Category;

namespace Business.Services.Abstract
{
    public interface ICategoryService
    {
        Task<CategoryIndexVM> GetAllAsync();
        Task<bool> CreateAsync(CategoryCreateVM model);
        Task<CategoryUpdateVM> UpdateAsync(int id);
        Task<bool> UpdateAsync(int id,CategoryUpdateVM model);
        Task<bool> DeleteAsync(int id);
    }
}
