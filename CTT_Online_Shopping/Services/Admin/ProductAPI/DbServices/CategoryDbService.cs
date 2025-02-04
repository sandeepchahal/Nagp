using MongoDB.Driver;
using ProductAPI.Helpers;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.DbServices;

public class CategoryDbService(IMongoCollection<CategoryDb> categoryCollection):ICategoryDbService
{
    public async Task<List<CategoryView>> GetAllAsync()
    {
        var categories = await categoryCollection.Find(_ => true).ToListAsync();
        return categories.Select(CategoryHelper.MapToCategoryView).ToList();
    }

    public async Task<CategoryView?> GetAsync(string categoryId)
    {
        var category = await categoryCollection.Find(col => col.Id == categoryId).FirstOrDefaultAsync();
        return category!= null ? CategoryHelper.MapToCategoryView(category) : null;
    }
}