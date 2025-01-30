namespace ProductConsumer.Models;

public class Product
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty; 
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ProductItemEventModel> Items { get; set; } = new();
}