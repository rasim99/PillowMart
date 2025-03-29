
using Business.Models.Admin.Product;
using Business.Models.Product;
using Core.Entities;

namespace Business.Services.Abstract
{
    public interface IProductService
    {
        Task<List<ProductVM>> GetProductsAsync();

        ProductCreateVM Create();
        Task<bool> CreateAsync(ProductCreateVM model);
        Task<ProductUpdateVM> UpdateAsync(int id);
        Task<bool> UpdateAsync(int id, ProductUpdateVM model);
        Task<bool> DeleteAsync(int id);
        List<Product> FilteredProduct(ProductFilterVM model);

    }
}
