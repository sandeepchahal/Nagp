namespace ProductAPI.Events.Models;

public class ProductItemRaiseEventModel
{
    public string EventType { get; init; } = string.Empty;
    public ProductItemAddEventModel ProductItem { get; set; } = null!;
}