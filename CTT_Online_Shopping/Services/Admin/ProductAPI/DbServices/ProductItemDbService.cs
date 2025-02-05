using MongoDB.Driver;
using ProductAPI.Helpers;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.DbServices;

public class ProductItemDbService(IMongoCollection<ProductItemDb> productItemCollection, IProductDbService productDbService):IProductItemDbService
{
    public async Task<ProductItemView?> GetAsync(string productItemId)
    {
        try
        {
            var productItem = await productItemCollection.FindAsync(col => col.Id == productItemId);
            var result = await productItem.FirstOrDefaultAsync();
            if (result is null)
                return null;
            var mapper = ProductItemMapper.MapToProductViewModel(result);
            var product = await productDbService.GetAsync(mapper.ProductId);
            mapper.Product = product is not null? ProductMapper.MapToProductView(product): new ProductView();
            return mapper;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}