using MongoDB.Driver;
using ProductAPI.Models;

namespace ProductAPI.DbServices;

public class ProductDbService(IMongoCollection<Product> productCollection,
    IProductItemDbService productItemDbService):IProductDbService
{
    public async Task<Product?> GetAsync(string id)
    {
        try
        {
            var result = await productCollection.FindAsync(p => p.Id == id);
            var product = await result.FirstOrDefaultAsync();
            if (product is null) return product;
            var productItems = await productItemDbService.GetByProductIdAsync(id);
            product.Items = productItems;
            return product;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}