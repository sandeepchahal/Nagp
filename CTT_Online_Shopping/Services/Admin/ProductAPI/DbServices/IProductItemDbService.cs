using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.DbServices;

public interface IProductItemDbService
{
    Task<ProductItemView?> GetAsync(string productItemId);
    Task<ProductItemDb> AddAsync(ProductItemCommand productItem);

    Task<int> RunAsync();
}