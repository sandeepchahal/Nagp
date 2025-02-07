using ProductAPI.Models.Query;

namespace ProductAPI.DbServices;

public interface IBrandDbService
{
    Task<BrandView> GetAsync(string brandId);
}