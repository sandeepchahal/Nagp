using ProductAPI.Models.DbModels;

namespace ProductAPI.Models.Abstract;

public abstract class ProductViewBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public BrandDb Brand { get; set; } = new BrandDb();
    
}