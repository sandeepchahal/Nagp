using ProductAPI.Models;

namespace ProductAPI.DbServices;

public interface IProductDbService
{
    Task<Product?> GetAsync(string id);
}