using ProductAPI.Models.Query;

namespace ProductAPI.DbServices;

public interface IProductItemDbService
{
    Task<ProductItemView?> GetAsync(string productItemId);
}