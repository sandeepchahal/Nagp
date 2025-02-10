using ProductAPI.Models.ProductItems;

namespace ProductAPI.Helper;

public static class ProductItemHelper
{
    public static ProductItemView MapToProductItemViewModel(ProductItem productItemDb)
    {
        return new ProductItemView()
        {
            ProductId = productItemDb.ProductId,
            VariantType = productItemDb.VariantType,
            Variants = productItemDb.Variants,
            Id = productItemDb.Id
        };
    } 
}