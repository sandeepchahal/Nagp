using ProductAPI.Enums;
using ProductAPI.Models.Abstract;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.Helpers;

public static class ProductItemMapper
{
    public static ProductItemDb MapToDomainModel(ProductItemCommand request)
    {
        return new ProductItemDb
        {
            ProductId = request.ProductId,
            VariantType = request.VariantType,
            Variant = new ProductVariantDb()
            {
                Discount = request.Variant.Discount,
                Images = request.Variant.Images,
                IsDiscountApplied = (Enum.TryParse(request.Variant.Discount.Type, out DiscountTypeEnum discountType) && discountType != DiscountTypeEnum.None),
                ColorVariant = request.Variant.ColorVariant is { Count: > 0 }
                    ? request.Variant.ColorVariant.Select(col => new ProductVariantColorDb()
                    {
                        Color = col.Color,
                        Discount = col.Discount,
                        Image = col.Image,
                        Price = col.Price,
                        StockQuantity = col.StockQuantity
                    }).ToList()
                    : null,
                SizeVariant = request.Variant.SizeVariant is { Count: > 0 }
                    ? request.Variant.SizeVariant.Select(col => new ProductVariantSizeDb()
                    {
                        Discount = col.Discount,
                        Price = col.Price,
                        StockQuantity = col.StockQuantity,
                        Size = col.Size
                    }).ToList()
                    : null,
                SizeColorVariant =
                    request.Variant.SizeColorVariant is { Count: > 0 }
                        ? request.Variant.SizeColorVariant.Select(col => new ProductVariantSizeColorDb()
                        {
                            Color = col.Color,
                            Sizes = col.Sizes,
                            Image = col.Image
                        }).ToList()
                        : null
            },
        };
    }

    public static ProductItemView MapToProductViewModel(ProductItemDb productItemDb)
    {
        return new ProductItemView()
        {
            ProductId = productItemDb.ProductId,
            VariantType = productItemDb.VariantType,
            Variant = productItemDb.Variant,
            Id = productItemDb.Id
        };
    }
    
}