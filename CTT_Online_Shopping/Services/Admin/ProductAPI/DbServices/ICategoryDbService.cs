using ProductAPI.Models.Abstract;
using ProductAPI.Models.Query;

namespace ProductAPI.DbServices;

public interface ICategoryDbService
{
    Task<List<CategoryView>> GetAllAsync();
    Task<CategoryView?> GetAsync(string categoryId);
}