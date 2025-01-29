namespace ProductConsumer.Models;

public class ProductItem
{
    public string Id { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public Dictionary<string, string> Attributes { get; set; } = null!;
    public decimal Price { get; set; } // Price of the product item
    public int Quantity { get; set; } // Available quantity
}

public class ProductItemConsumerModel
{
    public string EventType { get; init; } = string.Empty;
    public ProductItem ProductItem { get; set; } = null!;
}