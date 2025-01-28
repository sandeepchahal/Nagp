namespace ProductAPI.Models.Abstract;

public abstract class ProductItemBase
{
    public string ProductId { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public Dictionary<string, string> Attributes { get; set; } = null!;
    public decimal Price { get; set; } // Price of the product item
    public int Quantity { get; set; } // Available quantity
}