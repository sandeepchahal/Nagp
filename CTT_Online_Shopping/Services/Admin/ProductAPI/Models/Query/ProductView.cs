using ProductAPI.Models.Abstract;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Models.Query;

public class ProductView:ProductViewBase
{
    public ProductCategoryView Category { get; set; } = new();
    public List<ImagesBase> Images { get; set; } = new();
}
public class ProductDetailedView:ProductDb
{
    public List<ProductItemDb>? Items { get; set; } = new List<ProductItemDb>(); 
}

public class ProductCategoryView : CategoryViewBase
{
    public SubCategoryView SubCategory { get; set; } = new();
}