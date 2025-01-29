using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Models.Abstract;

public abstract class ProductItemBase
{
    [Required]
    public string ProductId { get; set; } = string.Empty;
    [Required]
    public string Sku { get; set; } = string.Empty;
    public Dictionary<string, string> Attributes { get; set; } = null!;
    [Required]
    public decimal Price { get; set; } // Price of the product item
    [Required]
    public int Quantity { get; set; } // Available quantity
}