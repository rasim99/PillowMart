
using Business.Models.Admin.Product;
using Business.Models.Product;
using Business.Services.Abstract;
using Core.Entities;
using Core.Utilities.File;
using Data.Repositories.Abstract;
using Data.UnitOfWork;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IProductRepistory _productRepistory;
        private readonly ICategoryRepistory _categoryRepistory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly string _rootPath;
        private readonly ModelStateDictionary _modelState;
        public ProductService(IProductRepistory productRepistory,
            IActionContextAccessor contextAccessor,
            ICategoryRepistory categoryRepistory,
            IUnitOfWork unitOfWork,
            IFileService fileService,
            string rootPath)
        {
            _productRepistory = productRepistory;
            _categoryRepistory = categoryRepistory;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _rootPath = rootPath;
            _modelState =contextAccessor.ActionContext.ModelState;
        }

        public async Task<List<ProductVM>> GetProductsAsync()
        {
            var productList = new List<ProductVM>();
            var products = _productRepistory.GetProductsWithCategory().ToList();

            foreach (var product in products)
            {
                var productVM = new ProductVM()
                {
                     Id = product.Id,
                      Description = product.Description,
                       PhotoName = product.PhotoName,
                       StockQuantity = product.StockQuantity,
                        Name = product.Name,
                         Price = product.Price,
                         CreatedDate = product.CreatedDate,
                          CategoryName=product.Category.Name
                };
                productList.Add(productVM);
            }
            return productList;
        }  


        public ProductCreateVM Create()
        {
            return new ProductCreateVM
            {
                Categories=_categoryRepistory.GetSelectListItems()
            };
        }
        public async Task<bool> CreateAsync(ProductCreateVM model)
        {
            model.Categories=_categoryRepistory.GetSelectListItems();
            if (!_modelState.IsValid) return false;
            var category = await _categoryRepistory.GetAsync(model.CategoryId);
            if (category == null)
            {
                _modelState.AddModelError("CategoryId", "Kategoriya tapılmadı");
                return false;
            }

            if(model.Photo is null)
            {
                _modelState.AddModelError("Photo", "foto seçilməli");
                return false;
            }

            if (!_fileService.IsImage(model.Photo.ContentType))
            {
                _modelState.AddModelError("Photo", "yanlış format");
                return false;
            }
            if (!_fileService.IsAvailableSize(model.Photo.Length))
            {
                _modelState.AddModelError("Photo", "foto həcmi böyükdür");
                return false;
            }

            var photoName = _fileService.Upload(model.Photo,_rootPath, "assets/images/products");

            var product = new Product
            {
                Name = model.Name,
                StockQuantity = model.StockQuantity,
                Description=model.Desciption,
                Price = model.Price,
                CategoryId = model.CategoryId,
                PhotoName = photoName
            };
          await  _productRepistory.CreateAsync(product);
            await _unitOfWork.CommitAsync();
           return true;
        }

        public async  Task<ProductUpdateVM> UpdateAsync(int id)
        {
          var product= await _productRepistory.GetAsync(id);
            if(product is null) return null;

            var model = new ProductUpdateVM
            {
                Name = product.Name,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                PhotoName= product.PhotoName,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Categories = _categoryRepistory.GetSelectListItems()
            };
            return model;
        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateVM model)
        {
            model.Categories =  _categoryRepistory.GetSelectListItems();
            var product = await _productRepistory.GetAsync(id);
            if (product is null)
            {
                _modelState.AddModelError(string.Empty, "Məhsul tapılmadı");
                return false;
            } 
            var category =  _categoryRepistory.GetAsync(model.CategoryId).Result;
            if (category is null)
            {
                _modelState.AddModelError(string.Empty, "Kateqoriya tapılmadı");
                return false;
            }
            
            if(!_modelState.IsValid) return false;
            product.Name = model.Name;
            product.Description=model.Description;
            product.StockQuantity = model.StockQuantity;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;
            if (model.Photo is not null)
            {
                if (!_fileService.IsImage(model.Photo.ContentType))
                {
                    _modelState.AddModelError("Photo", "Yanlış format");
                    return false;
                }
                if (!_fileService.IsAvailableSize(model.Photo.Length))
                {
                    _modelState.AddModelError("Photo", "foto həcmi böyükdür");
                    return false;
                }

                _fileService.Delete(_rootPath,"assets/images/products", product.PhotoName);

                var photoName = _fileService.Upload(model.Photo,_rootPath, "assets/images/products");
                product.PhotoName = photoName;
            }
            product.ModifiedDate = DateTime.Now;
            _productRepistory.Update(product);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepistory.GetAsync(id);
            if (product is null) return false;
            _fileService.Delete(_rootPath, "assets/images/products", product.PhotoName);
            _productRepistory.Delete(product);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public  List<Product> FilteredProduct(ProductFilterVM model)
        {
            
            var products = _productRepistory.GetProductsWithCategory();
            if (!string.IsNullOrEmpty(model.Name)) products = products.Where(p=>p.Name.Contains(model.Name));

            if(model.MinQuantity is not null) products = products.Where(p=>p.StockQuantity >= model.MinQuantity);
            if(model.MaxQuantity is not null) products = products.Where(p=>p.StockQuantity <= model.MaxQuantity);

            if(model.MinPrice is not null) products = products.Where(p=>p.Price >= model.MinPrice);
            if (model.MaxPrice is not null) products = products.Where(p => p.Price <= model.MaxPrice);

            if (model.CreatedStartDate is not null) products = products.Where(p => p.CreatedDate >= model.CreatedStartDate);
            if (model.CreatedEndDate is not null) products = products.Where(p => model.CreatedEndDate<=model.CreatedEndDate);

            if (model.CategoriesIds != null && model.CategoriesIds.Count > 0) products = products.Where(p => model.CategoriesIds.Contains(p.CategoryId));
            model.Categories = _categoryRepistory.GetSelectListItems();
            return products.ToList();
        }
    }
}
