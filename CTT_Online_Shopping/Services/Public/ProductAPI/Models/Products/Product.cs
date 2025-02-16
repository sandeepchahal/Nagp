using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductAPI.Models.Brands;
using ProductAPI.Models.Categories;
using ProductAPI.Models.Common;
using ProductAPI.Models.ProductItems;

namespace ProductAPI.Models.Products;

// brandname, productname, price, discount price, discount value, images,rating, 
// filters - Brand, price Range, color options, discount Range
// GetBrands from db
// get min and max price range from db where subcategory Id = id came in request
// get color options with counts
// show discount as enums may be 
// get attributes which are part of 

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
}

public class ProductWithSimilarGenderView
{
    public CategoryView Category { get; set; } = new();
    public List<ProductView> Products { get; set; } = new();
}

public class ProductWithSimilarChoiceView
{
    public SubCategoryView SubCategory { get; set; } = new();
    public List<ProductView> Products { get; set; } = new();
}

public class ProductWithSimilarBrand
{
    public Brand Brand { get; set; } = new();
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