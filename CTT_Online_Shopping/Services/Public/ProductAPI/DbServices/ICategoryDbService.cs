using ProductAPI.Models.Categories;

namespace ProductAPI.DbServices;

public interface ICategoryDbService
{
    Task<SubCategory?> GetSubCategoryAsync(string slug);
    Task<List<CategoryView>> GetAllCategories();
}