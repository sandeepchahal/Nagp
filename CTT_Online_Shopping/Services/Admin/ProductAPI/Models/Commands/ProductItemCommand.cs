using ProductAPI.Models.Abstract;

namespace ProductAPI.Models.Commands;

public class ProductItemCommand : ProductItemBase
{
    public List<ProductVariantBase> Variants { get; set; } = new();

}