using ProductAPI.Events.Models;
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
    
    public static ProductItemAddEventModel MapToProductItemEvent(ProductItemDb product)
    {
        var searchEvent = new ProductItemAddEventModel
        {
            ProductId = product.ProductId,
            Name = product.Name,
            MinPrice = product.Variants.Min(v => v.OriginalPrice), // Calculate min price
            MaxPrice = product.Variants.Max(v => v.OriginalPrice), // Calculate max price
            Attributes = product.Variants
                .SelectMany(v => v.Attributes
                    .Select(attr => $"{attr.Key}:{attr.Value}")) // Flatten attributes
                .Distinct()
                .ToList()
                .Distinct()
                .ToList()
        };

        return searchEvent;
    }
}