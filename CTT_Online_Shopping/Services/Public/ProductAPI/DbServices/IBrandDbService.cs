using ProductAPI.Models.Brands;

namespace ProductAPI.DbServices;

public interface IBrandDbService
{
    Task<List<Brand>> GetAll();
    Task<Brand?> GetAsync(string brandId);

}