using MongoDB.Driver;
using ProductAPI.Helpers;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.DbServices;

public class ProductDbService(IMongoCollection<ProductDb> productCollection, ICategoryDbService categoryDbService):IProductDbService
{
    public async Task<ProductView?> GetAsync(string productId)
    {
        try
        {
            var filter = Builders<ProductDb>.Filter.Eq(p => p.Id, productId);
            var product = await productCollection.Find(filter).FirstOrDefaultAsync();
            if (product == null)
            {
                return null;
            }

            var mapped = ProductMapper.MapToProductView(product);
            var category = await categoryDbService.GetAsync(product.CategoryId);
            mapped.Category = new ProductCategoryView()
            {
                Id = category!.Id,
                Gender = category.Gender,
                Name = category.Name,
                SubCategory = category.SubCategories.Where(col => col.Id == product.SubCategoryId).Select(col =>
                    new SubCategoryView()
                    {
                        Id = col.Id,
                        Name = col.Name
                    }).FirstOrDefault() ?? new SubCategoryView()
            };
            return mapped;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}