using ProductAPI.Models;
using ProductAPI.Models.Products;

namespace ProductAPI.DbServices;

public interface IProductDbService
{
    Task<Product?> GetAsync(string id);
    Task<List<Product>> GetAll();
    Task<List<ProductView>> GetBySubCategorySlugAsync(string slug);

    Task<List<ProductFilterView>> GetAsync(string? gender = null, string? brand = null, string? color = null,
        string? subcategory = null);




}