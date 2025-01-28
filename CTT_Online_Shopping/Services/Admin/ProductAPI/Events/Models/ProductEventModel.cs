using ProductAPI.Models.DbModels;

namespace ProductAPI.Events.Models;

public class ProductEventModel
{
    public string EventType { get; init; } = string.Empty;
    public ProductDb Product { get; init; } = null!;

}