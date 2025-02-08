using MongoDB.Driver;
using ProductAPI.Models.Brands;

namespace ProductAPI.DbServices;

public class BrandDbService( IMongoCollection<Brand> brandCollection):IBrandDbService

{
    public async Task<List<Brand>> GetAll()
    {
        try
        {
            var brands = await brandCollection.Find(_ => true)
                .Project(b => new Brand { Id = b.Id, Name = b.Name })
                .ToListAsync();
            return brands;
        }
        catch
        {
            throw;
        }
    }

    public async Task<Brand?> GetAsync(string brandId)
    {
        try
        {
            var brand = await brandCollection.Find(b => b.Id == brandId)
                .Project(b => new Brand { Id = b.Id, Name = b.Name })
                .FirstOrDefaultAsync();
            return brand;
        }
        catch
        {
            throw;
        }
    }
}