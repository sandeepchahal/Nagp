namespace ProductConsumer.Models;


public class ProductConsumerModel
{
    public string EventType { get; init; } = string.Empty;
    public ProductEventModel ProductEventModel { get; init; } = null!;
}
public class ProductEventModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}