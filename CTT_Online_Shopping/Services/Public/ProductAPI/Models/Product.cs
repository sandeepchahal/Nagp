namespace ProductAPI.Models;

public class Product
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ProductItem> Items { get; set; } = new List<ProductItem>(); 
}