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
        var discountAtProductLevelFlag = (Enum.TryParse(request.Variant.Discount.Type, out DiscountTypeEnum discountType) &&
                            discountType != DiscountTypeEnum.None);
        
        var result = new ProductItemDb
        {
            ProductId = request.ProductId,
            VariantType = request.VariantType,
            Variant = new ProductVariantDb()
            {
                Discount = request.Variant.Discount,
                Images = request.Variant.Images,
                IsDiscountApplied = discountAtProductLevelFlag,
                ColorVariant = request.Variant.ColorVariant is { Count: > 0 }
                    ? request.Variant.ColorVariant.Select(col => new ProductVariantColorDb()
                    {
                        Color = col.Color,
                        Discount = col.Discount,
                        Image = col.Image,
                        Price = col.Price,
                        StockQuantity = col.StockQuantity,
                    }).ToList()
                    : null,
                SizeVariant = request.Variant.SizeVariant is { Count: > 0 }
                    ? request.Variant.SizeVariant.Select(col => new ProductVariantSizeDb()
                    {
                        Discount = col.Discount,
                        Price = col.Price,
                        StockQuantity = col.StockQuantity,
                        Size = col.Size,
                    }).ToList()
                    : null,
                SizeColorVariant =
                    request.Variant.SizeColorVariant is { Count: > 0 }
                        ? request.Variant.SizeColorVariant.Select(col => new ProductVariantSizeColorDb()
                        {
                            Color = col.Color,
                            Sizes = col.Sizes.Select(col=>new ProductVariantSizeBase()
                            {
                                Discount = col.Discount,
                                Price = col.Price,
                                StockQuantity = col.StockQuantity,
                                Size = col.Size
                            }).ToList(),
                            Image = col.Image
                        }).ToList()
                        : null
            },
        };
        
        // calculate the discount
        if (result.Variant.SizeVariant != null)
            foreach (var sizeVariant in result.Variant.SizeVariant)
            {
                Enum.TryParse(result.Variant.Discount.Type, out DiscountTypeEnum sizeDiscountType);
                sizeVariant.DiscountedPrice =
                    CalculateDiscount(sizeDiscountType, sizeVariant.Price, sizeVariant.Discount);
                if (!discountAtProductLevelFlag) continue;
                Enum.TryParse(request.Variant.Discount.Type, out DiscountTypeEnum discountAtProuctLevelType);
                sizeVariant.DiscountedPrice =
                    CalculateDiscount(discountAtProuctLevelType, sizeVariant.DiscountedPrice, request.Variant.Discount);
            }
        if (result.Variant.ColorVariant != null)
            foreach (var colorVariant in result.Variant.ColorVariant)
            {
                Enum.TryParse(result.Variant.Discount.Type, out DiscountTypeEnum sizeDiscountType);
                colorVariant.DiscountedPrice =
                    CalculateDiscount(sizeDiscountType, colorVariant.Price, colorVariant.Discount);
                if (!discountAtProductLevelFlag) continue;
                Enum.TryParse(request.Variant.Discount.Type, out DiscountTypeEnum discountAtProuctLevelType);
                colorVariant.DiscountedPrice =
                    CalculateDiscount(discountAtProuctLevelType, colorVariant.DiscountedPrice, request.Variant.Discount);
            }
        
        if (result.Variant.SizeColorVariant != null)
            foreach (var sizeColorVariant in result.Variant.SizeColorVariant)
            {
                Enum.TryParse(request.Variant.Discount.Type, out DiscountTypeEnum sizeDiscountType);
                foreach (var size in sizeColorVariant.Sizes)
                {
                  size.DiscountedPrice=  CalculateDiscount(sizeDiscountType, size.Price, size.Discount);
                  if (!discountAtProductLevelFlag) continue;
                  Enum.TryParse(request.Variant.Discount.Type, out DiscountTypeEnum discountAtProuctLevelType);
                  size.DiscountedPrice =
                      CalculateDiscount(discountAtProuctLevelType, size.DiscountedPrice, request.Variant.Discount);
                }
            }

        return result;
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

    private static decimal CalculateDiscount(DiscountTypeEnum discountTypeEnum, decimal price, Discount discount)
    {
        if (discountTypeEnum == DiscountTypeEnum.Fixed)
        {
            return price - discount.Value;
        }
        else
        {
            return price - (discount.Value * price / 100);
        }
    }
    
}