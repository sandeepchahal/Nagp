using ProductAPI.Models.Abstract;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.Helpers;

public static class ProductMapper
{
    public static ProductView MapToProductView(ProductDb productDb)
    {
        return new ProductView()
        {
            Name = productDb.Name,
            Description = productDb.Description,
            Brand = productDb.Brand,
            Images = new List<ImagesBase>(),
            Id = productDb.Id
        };
    }
}