using MongoDB.Bson;
using MongoDB.Driver;
using ProductAPI.Helper;
using ProductAPI.Models.Brands;
using ProductAPI.Models.Categories;
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

    public async Task<List<ProductFilterView>> GetAsync(string? gender = null,
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
                { "localField", "categoryId" }, // categoryId from products
                { "foreignField", "_id" }, // Matching with _id in categories
                { "as", "CategoryData" }
            }),

            // Step 3: Unwind category data (if needed)
            new BsonDocument("$unwind", new BsonDocument
            {
                { "path", "$CategoryData" },
                { "preserveNullAndEmptyArrays", true } // Preserve if no category found
            }),

            // Step 6: Project only required fields
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 1 },
                { "name", 1 },
                { "description", 1 },
                { "brandId", 1 }
            })
        };

        // Execute the pipeline on `products` collection
        var results = await productCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

        List<ProductFilterView> finalResult = new();
        foreach (var doc in results)
        {
            var id = doc["_id"].ToString();
            var pf = new ProductFilterView()
            {
                Id = doc["_id"].ToString(),
                Name = doc["name"].ToString(),
                Description = doc["description"].ToString()
            };
            pf.Brand = await brandDbService.GetAsync(doc["brandId"].ToString()) ?? new Brand();
            pf.ProductItems = await FetchProductItemFilterContents(doc["_id"].ToString());
            finalResult.Add(pf);
        }

        return finalResult;
    }

    private async Task<List<ProductItemFilterContents>> FetchProductItemFilterContents(string productId)
    {
        var productItems = await productItemDbService.GetByProductIdAsync(productId);
        var result = new List<ProductItemFilterContents>();

        foreach (var item in productItems)
        {
            var productItemFilterContent = new ProductItemFilterContents()
            {
                Id = item.Id,
                Images = ProductHelper.GetImages(item),
                Price = ProductHelper.GetPrice(item)
            };
            result.Add(productItemFilterContent);
        }

        return result;
    }
}