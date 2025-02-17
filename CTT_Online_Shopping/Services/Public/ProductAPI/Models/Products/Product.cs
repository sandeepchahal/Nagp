using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductAPI.Models.Brands;
using ProductAPI.Models.Categories;
using ProductAPI.Models.Common;
using ProductAPI.Models.ProductItems;
using ProductAPI.Models.Review;

namespace ProductAPI.Models.Products;

public class PriceBase
{
    public decimal OriginalPrice { get; set; } 
    
    public int DiscountPrice { get; set; }

    public Discount Discount { get; set; } = new ();
}

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string BrandId { get; set; } = null!;  // Reference to Main Category
    public string CategoryId { get; set; } = null!;  // Reference to Main Category
    public string SubCategoryId { get; set; }= null!;  // Reference to Sub Category
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }= DateTime.Now;
}

public class ProductView
{
    public string Id { get; set; } = string.Empty;
    public string ProductItemId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Brand Brand { get; set; } = new();
    public PriceBase Price { get; set; } = new();
    public List<ImagesBase> Images { get; set; } = new();

    public List<ReviewQuery>? Reviews { get; set; }
}

public class ProductWithSimilarGenderView
{
    public CategoryView? Category { get; set; } = new();
    public List<ProductView> Products { get; set; } = new();
}

public class ProductWithSimilarChoiceView
{
    public SubCategoryView? SubCategory { get; set; } = new();
    public List<ProductView> Products { get; set; } = new();
}

public class ProductWithSimilarBrand
{
    public Brand? Brand { get; set; } = new();
    public List<ProductView> Products { get; set; } = new();
}

public class ProductItemFilterContents
{
    public string Id { get; set; } = string.Empty;
    public PriceBase Price { get; set; } = new();
    public List<ImagesBase> Images { get; set; } = new();
}

public class ProductFilterView
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    public Brand Brand { get; set; } = new();
    public List<ProductItemFilterContents> ProductItems { get; set; } = new();
}
