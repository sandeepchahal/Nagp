using MongoDB.Bson;
using MongoDB.Driver;
using ProductAPI.Helper;
using ProductAPI.Models.Brands;
using ProductAPI.Models.Categories;
using ProductAPI.Models.Common;
using ProductAPI.Models.ProductItems;
using ProductAPI.Models.Products;

namespace ProductAPI.DbServices;

public class ProductDbService(
    IMongoCollection<Product> productCollection,
    IMongoCollection<Category> categoryCollection,
    IProductItemDbService productItemDbService,
    
    IBrandDbService brandDbService,
    ICategoryDbService categoryDbService
) : IProductDbService
{
    public async Task<Product?> GetAsync(string id)
    {
        try
        {
            var result = await productCollection.FindAsync(p => p.Id == id);
            var product = await result.FirstOrDefaultAsync();
            if (product is null) return null;
            var productItems = await productItemDbService.GetByProductIdAsync(id);
            //product.Items = productItems;
            return product;
            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<Product>> GetAll()
    {
        try
        {
            var products = await productCollection.Find(_ => true).ToListAsync();
            return products;
        }
        catch (Exception e)
        {
            throw;
        }
        
    }

    public async Task<List<ProductView>> GetBySubCategorySlugAsync(string slug)
    {
        try
        {
            var subCategory = await categoryDbService.GetSubCategoryAsync(slug);
            if (subCategory == null)
                return new List<ProductView>();
            
            var productList = await productCollection.Find(col => col.SubCategoryId == subCategory.Id).ToListAsync();
            var result = new List<ProductView>();

            foreach (var product in productList)
            {
                var productView = ProductHelper.MapToProductView(product);

                // Brand info
                productView.Brand = await brandDbService.GetAsync(product.BrandId) ?? new Brand();
                // product items
                var productItems = await productItemDbService.GetByProductIdAsync(product.Id);
                foreach (var productItem in productItems)
                {
                    productView.Images = ProductHelper.GetImages(productItem!);
                    productView.Price = ProductHelper.GetPrice(productItem!);
                    productView.ProductItemId = productItem.Id;
                }

                result.Add(productView);
            }

            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<ProductView>> GetAsync(string? gender = null, 
        string? brand = null, 
        string? color = null, 
        string? subcategory = null)
    {
        var pipeline = new List<BsonDocument>
        {
            // Step 1: Match products with the given subCategoryId
            new BsonDocument("$match", new BsonDocument
            {
                { "subCategoryId", subcategory }
            }),

            // Step 2: Lookup category details for each product
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "categories" },  
                { "localField", "categoryId" },   // categoryId from products
                { "foreignField", "_id" },        // Matching with _id in categories
                { "as", "CategoryData" }
            }),

            // Step 3: Unwind category data (if needed)
            new BsonDocument("$unwind", new BsonDocument
            {
                { "path", "$CategoryData" },
                { "preserveNullAndEmptyArrays", true }  // Preserve if no category found
            }),

            // Step 4: Project only required fields
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 1 },
                { "name", 1 },
                { "CategoryData.name", 1 },  // Include category name (optional)
                { "subCategoryId", 1 }
            })  
        };

        // Execute the pipeline on `products` collection
        var results = await productCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

        // Convert results into ProductView
        var finalProducts = results.Select(p => new ProductView
        {
            Id = p["_id"].AsString,
            Name = p["name"].AsString,
            
        }).ToList();

        return finalProducts;    }
}