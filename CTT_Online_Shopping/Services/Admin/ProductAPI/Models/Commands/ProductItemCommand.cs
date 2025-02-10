using ProductAPI.Models.Abstract;

namespace ProductAPI.Models.Commands;

public class ProductItemCommand : ProductItemBase
{
    public ProductVariantCommand Variant { get; set; } = new();
}

public class ProductVariantCommand:ProductVariantBase
{
    public List<ProductVariantSizeWithImage>? SizeVariant { get; set; }  // Nullable Size Variant
    public List<ProductVariantColorBase>? ColorVariant { get; set; }  // Nullable Color Variant
    public List<ProductVariantSizeColorCommand>? SizeColorVariant { get; set; }  // Nullable Size-Color Variant
}

public class ProductVariantSizeColorCommand : ProductVariantSizeColorBase
{
    public List<ProductVariantSizeBase> Sizes { get; set; } = new();
}