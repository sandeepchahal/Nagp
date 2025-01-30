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

public abstract class ProductVariantBase
{
    public Dictionary<string, string> Attributes { get; set; } = new(); // E.g., {"Color": "Red", "Size": "M"}
    public int RemainingStockQuantity { get; set; }  // Stock per variant remaining
    public decimal OriginalPrice { get; set; }  // Price per variant
    public List<ImagesBase> Images { get; set; } = new();
    public Discount? Discount { get; set; }
}

public class ProductVariant : ProductVariantBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public bool IsDiscountApplied { get; set; } = false;
}

public abstract class ImagesBase
{
    public string Url { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public bool IsPrimary { get; set; } = false;
    public int OrderNumber { get; set; }
}
public abstract class Discount
{
    public string Type { get; set; } = nameof(DiscountEnum.Percentage);
    public decimal Value { get; set; } 
}
