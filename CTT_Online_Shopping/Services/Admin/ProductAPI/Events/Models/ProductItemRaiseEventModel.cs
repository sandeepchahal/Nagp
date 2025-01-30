namespace ProductAPI.Events.Models;

public class ProductItemRaiseEventModel
{
    public string EventType { get; init; } = string.Empty;
    public ProductItemEventModel ProductItem { get; set; } = null!;
}