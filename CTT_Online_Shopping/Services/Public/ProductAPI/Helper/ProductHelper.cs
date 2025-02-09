using ProductAPI.Models.Products;

namespace ProductAPI.Helper;

public static class ProductHelper
{
    public static ProductView MapToProductView(Product product)
    {
        return new ProductView()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description
        };
    }
}