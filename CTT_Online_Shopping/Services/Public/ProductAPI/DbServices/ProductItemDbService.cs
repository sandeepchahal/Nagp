using MongoDB.Driver;
using ProductAPI.Models;
using ProductAPI.Models.ProductItems;

namespace ProductAPI.DbServices;

public class ProductItemDbService(IMongoCollection<ProductItem> productItemCollection):IProductItemDbService
{
    public async Task<List<ProductItem>?> GetByProductIdAsync(string productId)
    {
        try
        {
            var result =
                await productItemCollection.Find(p => p.ProductId == productId).ToListAsync();
            return result;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ProductItem?> GetAsync(string id)
    {
        try
        {
            var result =
                await productItemCollection.FindAsync(p => p.Id == id);
            return await result.FirstOrDefaultAsync();

        }
        catch (Exception)
        {
            throw;
        }
    }
}