using ProductAPI.Models.Query;

namespace ProductAPI.DbServices;

public interface IProductDbService
{
    Task<ProductView?> GetAsync(string productId);
}