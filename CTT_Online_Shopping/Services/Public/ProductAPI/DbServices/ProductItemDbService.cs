using MongoDB.Driver;
using ProductAPI.Helper;
using ProductAPI.Models;
using ProductAPI.Models.ProductItems;
using ProductAPI.Models.Products;

namespace ProductAPI.DbServices;

public class ProductItemDbService(
    IMongoCollection<ProductItem> productItemCollection ):IProductItemDbService
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

    public async Task<ProductItemView?> GetAsync(string productItemId)
    {
        try
        {
            var productItem = await productItemCollection.FindAsync(col => col.Id == productItemId);
            var result = await productItem.FirstOrDefaultAsync();
            if (result is null)
                return null;
            var mapper = ProductItemHelper.MapToProductItemViewModel(result);
            return mapper;
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    public async Task<List<ProductItem>> GetAllAsync()
    {
        try
        {
            var result =
                await productItemCollection.Find(_ => true).ToListAsync();
            return result;

        }
        catch (Exception)
        {
            throw;
        }
    }
}