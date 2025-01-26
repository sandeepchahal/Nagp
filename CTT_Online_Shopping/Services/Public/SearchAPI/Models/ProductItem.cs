namespace SearchAPI.Models;
public class ProductItem
{
    public string Id { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public Dictionary<string, string> Attributes { get; set; } = null!;
    public decimal Price { get; set; } // Price of the product item
    public int Quantity { get; set; } // Available quantity
}
