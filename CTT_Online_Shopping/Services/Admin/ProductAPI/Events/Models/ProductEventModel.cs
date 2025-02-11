using ProductAPI.Models.DbModels;

namespace ProductAPI.Events.Models;

public class ProductUpdateEventModel
{
    // Adding product event is not there as product item is required to show the result to the user

    public string ProductId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Brand { get; set; } = null!;
}