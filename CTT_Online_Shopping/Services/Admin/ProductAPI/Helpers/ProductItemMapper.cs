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
            ProductId = request.ProductId,
            ProductLevelDiscount = request.ProductLevelDiscount,
            Variants = request.Variants.Select(v => new ProductVariant
            {
                Attributes = v.Attributes,
                Discount = v.Discount,
                Images = v.Images,
                IsDiscountApplied = false,
            }).ToList()
        };
    }
    
    // public static ProductItemEventModel MapToProductItemEvent(ProductItemDb product)
    // {
    //     var searchEvent = new ProductItemEventModel
    //     {
    //         Id = product.Id,
    //         ProductId = product.ProductId,
    //         Name = product.Name,
    //         MinPrice = product.Variants.Min(v => v.), // Calculate min price
    //         MaxPrice = product.Variants.Max(v => v.OriginalPrice), // Calculate max price
    //         Attributes = product.Variants
    //             .SelectMany(v => v.Attributes
    //                 .Select(attr => $"{attr.Key}:{attr.Value}")) // Flatten attributes
    //             .Distinct()
    //             .ToList()
    //             .Distinct()
    //             .ToList()
    //     };
    //
    //     return searchEvent;
    // }
}