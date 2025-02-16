using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ProductAPI.Helper;
using ProductAPI.Models.Categories;

namespace ProductAPI.DbServices;

public class CategoryDbService(IMongoCollection<Category> categoryCollection):ICategoryDbService
{
    public async Task<SubCategory?> GetSubCategoryAsync(string slug)
    {
        var filter = Builders<Category>.Filter.ElemMatch<SubCategory>("subCategories",
            Builders<SubCategory>.Filter.Eq("slug", slug));

        var projection = Builders<Category>.Projection.Include("subCategories.$"); // Fetch only matched subCategory

        var result = await categoryCollection.Find(filter).Project(projection).FirstOrDefaultAsync();
        // Check if the result is not null
        if (result == null) return null;
        // Extract the matched subCategory (it will be in the 'subCategories' field of the document)
        var subCategoryBson = result["subCategories"].AsBsonArray.FirstOrDefault();

        if (subCategoryBson == null) return null;
        // Deserialize it into your SubCategory model
        var subCategory = BsonSerializer.Deserialize<SubCategory>(subCategoryBson.AsBsonDocument);
        
        return subCategory;
    }

    public async Task<SubCategory?> GetSubCategoryIdAsync(string subcategoryId)
    {
        var filter = Builders<Category>.Filter.ElemMatch<SubCategory>("subCategories",
            Builders<SubCategory>.Filter.Eq("_id", subcategoryId));

        var projection = Builders<Category>.Projection.Include("subCategories.$"); // Fetch only matched subCategory

        var result = await categoryCollection.Find(filter).Project(projection).FirstOrDefaultAsync();
        // Check if the result is not null
        if (result == null) return null;
        // Extract the matched subCategory (it will be in the 'subCategories' field of the document)
        var subCategoryBson = result["subCategories"].AsBsonArray.FirstOrDefault();

        if (subCategoryBson == null) return null;
        // Deserialize it into your SubCategory model
        var subCategory = BsonSerializer.Deserialize<SubCategory>(subCategoryBson.AsBsonDocument);
        
        return subCategory;
    }

    public async Task<List<CategoryView>> GetAllCategories()
    {
        var result = await categoryCollection.Find(_ => true).ToListAsync();

        return result.Select(category => CategoryHelper.MapToCategoryView(category)).ToList();
    }

    public async Task<CategoryView> GetAsync(string categoryId)
    {
        var result = await categoryCollection.Find(col => col.Id == categoryId).FirstOrDefaultAsync();
        return CategoryHelper.MapToCategoryView(result);
    }
}