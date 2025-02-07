using MongoDB.Driver;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.DbServices;

public class BrandDbService(IMongoCollection<BrandDb> brandCollection):IBrandDbService
{
    public async Task<BrandView> GetAsync(string brandId)
    {
        var brand = await brandCollection.Find(b => b.Id == brandId)
            .Project(b => new BrandView { Id = b.Id, Name = b.Name })
            .FirstOrDefaultAsync();
        return brand?? new BrandView();
    }
}