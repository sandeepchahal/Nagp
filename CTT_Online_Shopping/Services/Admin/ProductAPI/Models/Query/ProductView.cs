using ProductAPI.Models.Abstract;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Models.Query;

public class ProductView:ProductDb
{
    public List<ProductItemDb>? Items { get; set; } = new List<ProductItemDb>(); 
}