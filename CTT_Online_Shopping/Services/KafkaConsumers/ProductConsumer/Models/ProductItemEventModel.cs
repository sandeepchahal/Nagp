namespace ProductConsumer.Models;

public class ProductItemEventModel
{
    public string Id { get; set; } = null!;
    public string ProductId { get; set; } = string.Empty; 
    public string Name { get; set; } = string.Empty; 
    public decimal MinPrice { get; set; } 
    public decimal MaxPrice { get; set; }

    public List<string> Attributes { get; set; } =
        new();
}


public class ProductItemConsumerModel
{
    public string EventType { get; init; } = string.Empty;
    public ProductItemEventModel ProductItemEventModel { get; set; } = null!;
}