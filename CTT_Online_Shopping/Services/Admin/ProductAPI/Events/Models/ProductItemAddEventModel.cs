namespace ProductAPI.Events.Models;

public class ProductItemAddEventModel
{
    public string Id { get; set; } = null!;
    public string ProductId { get; set; } = string.Empty; // Required for unique identification
    public string Name { get; set; } = string.Empty; // Required for search
    public string Description { get; set; } = string.Empty; // Optional for search
    public decimal MinPrice { get; set; } // Minimum price across all variants (for filtering)
    public decimal MaxPrice { get; set; } // Maximum price across all variants (for filtering)

    public List<string> Attributes { get; set; } =
        new(); // Flattened attributes for search (e.g., ["Color:Red", "Size:M"])
}