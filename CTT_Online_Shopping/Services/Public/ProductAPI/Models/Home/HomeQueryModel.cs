using ProductAPI.Models.Products;

namespace ProductAPI.Models.Home;

public class HomeQueryModel
{
    public List<ProductView> Men { get; set; } = new();
    public List<ProductView> Women { get; set; } = new();

}