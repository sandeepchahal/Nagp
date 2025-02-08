using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductAPI.Models.Common;

namespace ProductAPI.Models.ProductItems;

public class ProductVariantSizeBase
{
    public string Size { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public Discount? Discount { get; set; }
    public decimal DiscountedPrice { get; set; }
}
public class ProductVariantBase
{
    public Discount Discount { get; set; } = new();
    public List<ImagesBase>? Images { get; set; }
}

public class ProductVariantSize
{
    public string Id { get; set; } = null!;
    public string Size { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public Discount Discount { get; set; } = new();
    public decimal DiscountedPrice { get; set; }
    public List<ImagesBase> Images { get; set; } = new();
}
public class ProductVariantColor
{
    public string Id { get; set; }
    public decimal DiscountedPrice { get; set; }
    public string Color { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public Discount? Discount { get; set; }
    public ImagesBase Image { get; set; } = new();
}

public class ProductVariantSizeColor
{
    public string Id { get; set; }
    public string Color { get; set; } = string.Empty;
    public ImagesBase Image { get; set; } = new();
    public List<ProductVariantSizeBase> Sizes { get; set; } = new();
}
public class ProductVariant : ProductVariantBase
{
    
    public string Id { get; set; }
    public bool IsDiscountApplied { get; set; } = false;
    public List<ProductVariantSize>? SizeVariant { get; set; }  // Nullable Size Variant
    public List<ProductVariantColor>? ColorVariant { get; set; }  // Nullable Color Variant
    public List<ProductVariantSizeColor>? SizeColorVariant { get; set; }  // Nullable Size-Color Variant
}
public class ProductItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string ProductId { get; set; } = string.Empty;
    public string VariantType { get; set; } = string.Empty;
    public ProductVariant Variants { get; set; } = new();
    
}

