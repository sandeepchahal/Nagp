using ProductAPI.Models.DbModels;

namespace ProductAPI.Events.Models;

public class ProductItemEventModel
{
    public string EventType { get; init; } = string.Empty;
    public ProductItemDb ProductItem { get; set; } = null!;
}