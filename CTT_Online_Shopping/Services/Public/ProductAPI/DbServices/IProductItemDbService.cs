using ProductAPI.Models;

namespace ProductAPI.DbServices;

public interface IProductItemDbService
{
    Task<List<ProductItem>?> GetByProductIdAsync(string productId);
    Task<ProductItem?> GetAsync(string id);

}