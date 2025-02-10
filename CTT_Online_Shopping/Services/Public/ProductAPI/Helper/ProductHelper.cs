using ProductAPI.Models.Common;
using ProductAPI.Models.ProductItems;
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
     public static List<ImagesBase> GetImages(ProductItem productItem)
    {
        if (productItem?.Variants == null)
            return new List<ImagesBase>();

        var images = new List<ImagesBase>();

        switch (productItem.VariantType)
        {
            case "Size":
                if (productItem.Variants.SizeVariant != null)
                {
                    images.AddRange(productItem.Variants.Images);
                 
                }

                break;

            case "Color":
                if (productItem.Variants.ColorVariant != null)
                {
                    images.AddRange(productItem.Variants.ColorVariant.Select(colorVariant => colorVariant.Image));
                }

                break;

            case "ColorAndSize":
                if (productItem.Variants.SizeColorVariant != null)
                {
                    images.AddRange(from sizeColorVariant in productItem.Variants.SizeColorVariant
                        where sizeColorVariant.Image.Url.Length > 0
                        select sizeColorVariant.Image);
                }

                break;

            default:
                break;
        }

        return images;
    }


    public static PriceBase GetPrice(ProductItem productItem)
    {
        if (productItem?.Variants == null)
            return new PriceBase();

        switch (productItem.VariantType)
        {
            case "Size":
                if (productItem.Variants.SizeVariant != null)
                {
                    var sizeVariant = productItem.Variants.SizeVariant.FirstOrDefault();
                    if (sizeVariant != null)
                    {
                        return new PriceBase
                        {
                            OriginalPrice = sizeVariant.Price,
                            DiscountPrice = (int)sizeVariant.DiscountedPrice,
                            Discount = sizeVariant.Discount ?? new Discount()
                        };
                    }
                }

                break;

            case "Color":
                if (productItem.Variants.ColorVariant != null)
                {
                    var colorVariant = productItem.Variants.ColorVariant.FirstOrDefault();
                    if (colorVariant != null)
                    {
                        return new PriceBase
                        {
                            OriginalPrice = colorVariant.Price,
                            DiscountPrice = (int)colorVariant.DiscountedPrice,
                            Discount = colorVariant.Discount ?? new Discount()
                        };
                    }
                }

                break;

            case "ColorAndSize":
                if (productItem.Variants.SizeColorVariant != null)
                {
                    var sizeColorVariant = productItem.Variants.SizeColorVariant.FirstOrDefault();
                    if (sizeColorVariant != null && sizeColorVariant.Sizes.Any())
                    {
                        var sizeVariant = sizeColorVariant.Sizes.First();
                        return new PriceBase
                        {
                            OriginalPrice = sizeVariant.Price,
                            DiscountPrice = (int)sizeVariant.DiscountedPrice,
                            Discount = sizeVariant.Discount ?? new Discount()
                        };
                    }
                }

                break;

            default:
                break;
        }

        return new PriceBase();
    }
}