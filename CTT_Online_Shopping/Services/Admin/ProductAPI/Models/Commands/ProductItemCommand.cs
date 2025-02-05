using ProductAPI.Models.Abstract;

namespace ProductAPI.Models.Commands;

public class ProductItemCommand : ProductItemBase
{
    public ProductVariantCommand Variant { get; set; } = new();
}

public class ProductVariantCommand:ProductVariantBase
{
    public List<ProductVariantSizeBase>? SizeVariant { get; set; }  // Nullable Size Variant
    public List<ProductVariantColorBase>? ColorVariant { get; set; }  // Nullable Color Variant
    public List<ProductVariantSizeColorBase>? SizeColorVariant { get; set; }  // Nullable Size-Color Variant
}