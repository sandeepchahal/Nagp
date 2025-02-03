using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductAPI.Enums;

namespace ProductAPI.Models.Abstract;

public abstract class ProductItemBase
{
    [Required]
    public string ProductId { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    public Discount? ProductLevelDiscount { get; set; }
}

public  class ProductVariantBase
{
    public List<ProductFeaturesBase> Attributes { get; set; } = new(); 
    public List<ImagesBase> Images { get; set; } = new();
    public Discount? Discount { get; set; }
}

public abstract class ProductFeaturesBase
{
    public Dictionary<string, string> Features { get; set; } = new(); 
    public int StockQuantity { get; set; }  // Stock per variant remaining
    public decimal Price { get; set; }  // Price per variant
}
public class ProductVariant : ProductVariantBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public bool IsDiscountApplied { get; set; } = false;
}

public class ImagesBase
{
    public string Url { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public bool IsPrimary { get; set; } = false;
    public int OrderNumber { get; set; }
}
public class Discount
{
    public string Type { get; set; } = nameof(DiscountEnum.Percentage);
    public decimal Value { get; set; } 
}
