namespace ProductConsumer.Models;

public class ProductItemConsumerModel
{
    public string EventType { get; init; } = string.Empty;
    public ProductItemEventModel ProductItem { get; set; } = null!;
}

public class ProductItemEventModel
{
    public string ProductItemId { get; set; } = null!;
    public string ProductId { get; set; } = string.Empty; // Required for unique identification
    public string Name { get; set; } = string.Empty; // Required for search
    public string Brand { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string SubCategoryName { get; set; } = string.Empty;
    public string SubCategoryId { get; set; } = string.Empty;
    public string SubCategorySlug { get; set; } = string.Empty;
    public string VariantType { get; set; } = string.Empty;
    public decimal MinPrice { get; set; } // Minimum price across all variants (for filtering)
    public decimal MaxPrice { get; set; } // Maximum price across all variants (for filtering)
    public List<ProductVariantSizeEventModel>? SizeVariant { get; set; }  // Nullable Size Variant
    public List<ProductVariantColorEventModel>? ColorVariant { get; set; }  // Nullable Color Variant
    public List<ProductVariantSizeAndColorEventModel>? SizeColorVariant { get; set; }  // Nullable Size-Color Variant
}


public class ProductVariantSizeEventModel
{
    public string SizeId { get; set; } = null!;
    public string Size { get; set; } = null!;
}
public class ProductVariantColorEventModel
{
    public string ColorId { get; set; }=null!;
    public string Color { get; set; } = null!;
}
public class ProductVariantSizeAndColorEventModel
{
    public string SizeAndColorId { get; set; }=null!;
    public ProductVariantColorEventModel Color { get; set; } = null!;
    public List<ProductVariantSizeEventModel> Sizes { get; set; } = null!;
}