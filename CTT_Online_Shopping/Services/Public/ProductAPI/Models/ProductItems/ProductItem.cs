using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductAPI.Models.Common;
using ProductAPI.Models.Products;

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
    [BsonElement("discount")]
    public Discount Discount { get; set; } = new();
    [BsonElement("images")]
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
    
    [BsonElement("images")]
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
    
    [BsonElement("image")]
    public ImagesBase Image { get; set; } = new();
}

public class ProductVariantSizeColor
{
    
    public string Id { get; set; }
    
    [BsonElement("color")]
    public string Color { get; set; } = string.Empty;
    
    [BsonElement("image")]
    public ImagesBase Image { get; set; } = new();
    
    [BsonElement("sizes")]
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
    
    [BsonElement("productId")]
    public string ProductId { get; set; } = string.Empty;
    
    [BsonElement("variantType")]
    public string VariantType { get; set; } = string.Empty;
    
    [BsonElement("variants")]
    public ProductVariant Variants { get; set; } = new();
}

public class ProductItemView : ProductItem
{
    public ProductView Product { get; set; } = new ();
}
