using ProductAPI.Models.DbModels;

namespace ProductAPI.Models.Query;

public class ProductItemView : ProductItemDb
{
    public ProductView Product { get; set; } = new ProductView();
}