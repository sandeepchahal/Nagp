using System.ComponentModel.DataAnnotations;
using ProductAPI.Enums;

namespace ProductAPI.Models.Abstract;

public abstract class ProductItemBase
{
    [Required]
    public string ProductId { get; set; } = string.Empty;
    public string VariantType { get; set; } = string.Empty;
}

public class ProductVariantBase
{ 
    public List<ImagesBase>? Images { get; set; } // only add images when going by size
    public Discount Discount { get; set; } = new();
}

public class ProductVariantDb : ProductVariantBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public bool IsDiscountApplied { get; set; } = false;
    public List<ProductVariantSizeDb>? SizeVariant { get; set; }  // Nullable Size Variant
    public List<ProductVariantColorDb>? ColorVariant { get; set; }  // Nullable Color Variant
    public List<ProductVariantSizeColorDb>? SizeColorVariant { get; set; }  // Nullable Size-Color Variant
}

public class ImagesBase
{
    public string Url { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public int OrderNumber { get; set; }
}
public class Discount
{
    public string Type { get; set; } = string.Empty;
    public decimal Value { get; set; } 
}

public class ProductVariantSizeColorBase
{
    public string Color { get; set; } = string.Empty;
    public ImagesBase Image { get; set; } = new ImagesBase();
    public List<ProductVariantSizeBase> Sizes { get; set; } = new();
}
public class ProductVariantColorBase
{
    public string Color { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public Discount? Discount { get; set; }
    public ImagesBase Image { get; set; } = new ImagesBase();
}
public class ProductVariantSizeBase
{
    public string Size { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public Discount? Discount { get; set; }
}

public class ProductVariantSizeColorDb:ProductVariantSizeColorBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
public class ProductVariantColorDb:ProductVariantColorBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
public class ProductVariantSizeDb:ProductVariantSizeBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}