using ProductAPI.Models.Abstract;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Helpers;

public static class ProductItemMapper
{
    public static ProductItemDb MapToDomainModel(ProductItemCommand request)
    {
        return new ProductItemDb
        {
            Name = request.Name,
            ProductLevelDiscount = request.ProductLevelDiscount,
            Variants = request.Variants.Select(v => new ProductVariant
            {
                Attributes = v.Attributes,
                RemainingStockQuantity = v.RemainingStockQuantity,
                OriginalPrice = v.OriginalPrice,
                Discount = v.Discount,
                Images = v.Images
            }).ToList()
        };
    }
}