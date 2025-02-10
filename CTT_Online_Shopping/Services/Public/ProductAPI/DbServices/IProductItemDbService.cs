using ProductAPI.Models;
using ProductAPI.Models.ProductItems;

namespace ProductAPI.DbServices;

public interface IProductItemDbService
{
    Task<List<ProductItem>?> GetByProductIdAsync(string productId);
    Task<ProductItemView?> GetAsync(string id);
    Task<List<ProductItem>> GetAllAsync();

}