
using Business.Models.Admin.Category;
using Business.Services.Abstract;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Business.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepistory _categoryRepistory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ModelStateDictionary _modelState;
        public CategoryService(ICategoryRepistory categoryRepistory,
                           IActionContextAccessor contextAccessor,
                           IUnitOfWork unitOfWork)
        {
            _modelState=contextAccessor.ActionContext.ModelState;
            _categoryRepistory = categoryRepistory;
            _unitOfWork = unitOfWork;
        }
        public async Task<CategoryIndexVM> GetAllAsync()
        {
            return new CategoryIndexVM
            {
                Categories = await _categoryRepistory.GetAllAsync(),
            };
        }

        public async Task<bool> CreateAsync(CategoryCreateVM model)
        {
            if (!_modelState.IsValid) return false;
            var category =await _categoryRepistory.GetByNameAsync(model.Name);
            if (category is not null)
            {
                _modelState.AddModelError("Name", "Artıq mövcuddur!");
                return false;
            }
            category = new Category
            {
                Name = model.Name,
                CreatedDate = DateTime.Now
            };

            await _categoryRepistory.CreateAsync(category);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<CategoryUpdateVM> UpdateAsync(int id)
        {
            var category = await _categoryRepistory.GetAsync(id);
            if (category is null) return null;
            var model = new CategoryUpdateVM
            {
                Name = category.Name
            };
            return model;
        }

        public async Task<bool> UpdateAsync(int id, CategoryUpdateVM model)
        {
            var category =await _categoryRepistory.GetAsync(id);
            if (category is null)
            {
                _modelState.AddModelError(string.Empty, "Kateqoriya tapılmadı!");
                return false;
            }
            var existCategory = await _categoryRepistory.GetByNameAsync(model.Name);
            if (existCategory is not null && existCategory.Id!=id)
            {
                _modelState.AddModelError("Name", "Eyniadlı kateqoriya artıq mövcuddur");
                return false;
            }
            category.Name = model.Name;
            category.ModifiedDate = DateTime.Now;
            _categoryRepistory.Update(category);
           await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category =await _categoryRepistory.GetAsync(id);
            if (category is null) return false;
            _categoryRepistory.Delete(category);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
