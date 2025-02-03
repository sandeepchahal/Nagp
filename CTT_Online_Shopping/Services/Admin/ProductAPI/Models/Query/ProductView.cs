using ProductAPI.Models.Abstract;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Models.Query;

public class ProductView:ProductDb
{
    public decimal DiscountPrice { get; set; }
    public decimal OriginalPrice { get; set; }
    public string OverallRating { get; set; } = string.Empty;
    public decimal NumberOfReviews { get; set; }
    public List<ImagesBase> Images { get; set; } = new();
}
public class ProductDetailedView:ProductDb
{
    public List<ProductItemDb>? Items { get; set; } = new List<ProductItemDb>(); 
}