using ProductAPI.Models;
using ProductAPI.Models.ProductItems;
using ProductAPI.Models.Products;

namespace ProductAPI.DbServices;

public interface IProductItemDbService
{
    Task<List<ProductItem>?> GetByProductIdAsync(string productId);
    Task<ProductItemView?> GetAsync(string id);
    Task<List<ProductItem>> GetAllAsync();
    Task<ProductWithSimilarBrand> GetByBrand(string brandId);
    Task<ProductWithSimilarGenderView> GetByCategoryType(string categoryId);
    Task<ProductWithSimilarChoiceView> GetBySubcategoryType(string subcategoryId);

}