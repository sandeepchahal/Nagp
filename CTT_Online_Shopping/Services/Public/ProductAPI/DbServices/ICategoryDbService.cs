using ProductAPI.Models.Categories;

namespace ProductAPI.DbServices;

public interface ICategoryDbService
{
    Task<SubCategory?> GetSubCategoryAsync(string slug);
    Task<SubCategory?> GetSubCategoryIdAsync(string subcategoryId);
    Task<List<CategoryView>> GetAllCategories();

    Task<CategoryView> GetAsync(string categoryId);
}