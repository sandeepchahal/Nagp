using MongoDB.Bson;
using MongoDB.Driver;
using ProductAPI.Extensions;
using ProductAPI.Helper;
using ProductAPI.Models.Brands;
using ProductAPI.Models.ProductItems;
using ProductAPI.Models.Products;
using ProductAPI.Models.WishList;

namespace ProductAPI.DbServices;

public class ProductDbService(
    IMongoCollection<Product> productCollection,
    IProductItemDbService productItemDbService,
    IBrandDbService brandDbService,
    ICategoryDbService categoryDbService,
    IMongoCollection<WishListDb> wishListCollection) : IProductDbService
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
        var results = await productCollection.Aggregate<Product>(pipeline).ToListAsync();

        List<ProductFilterView> finalResult = new();
        foreach (var doc in results)
        {
            var pf = new ProductFilterView()
            {
                Id = doc.Id,
                Name = doc.Name,
                Description = doc.Description
            };
            pf.Brand = await brandDbService.GetAsync(doc.BrandId) ?? new Brand();
            pf.ProductItems = await FetchProductItemFilterContents(doc.Id, color);
            if (pf.ProductItems.Any())
            {
                finalResult.Add(pf);
            }
            
        }

        return finalResult;
    }

    public async Task<List<WishListQuery>> GetWishlist(string email)
    {
        
        var result = await wishListCollection.Find(col => col.Email == email).ToListAsync();

        List<WishListQuery> list = new();
        foreach (var item in result)
        {
            var product = await GetAsync(item.ProductId);
            var brand = await brandDbService.GetAsync(product.BrandId);
            var productItem = await productItemDbService.GetAsync(item.ProductItemId);
            var image = ProductHelper.GetImages(productItem);
            var price = ProductHelper.GetPrice(productItem);
                var wishList = new WishListQuery()
            {
                ProductItemId = item.ProductItemId,
                ProductId = item.ProductId,
                Id = item.Id,
                Name = product.Name,
                Brand = brand,
                ImageUrl = image[0].Url,
                Price = price.OriginalPrice==0?price.DiscountPrice: price.OriginalPrice.ToInt()
            };
            list.Add(wishList);
        }

        return list;
    }

    private async Task<List<ProductItemFilterContents>> FetchProductItemFilterContents(string productId, string? color= null)
    {
        var productItems = await productItemDbService.GetByProductIdAsync(productId);
        var result = new List<ProductItemFilterContents>();

        foreach (var item in productItems)
        {
            if (color != null)
            {   
                if(item.VariantType == "Size") continue;
                if (!DoesColorMatch(item, color)) continue;
                var productItemFilterContent = new ProductItemFilterContents()
                {
                    Id = item.Id,
                    Images = ProductHelper.GetImages(item),
                    Price = ProductHelper.GetPrice(item)
                };
                result.Add(productItemFilterContent);
            }
            else if (color == null)
            {
                var productItemFilterContent = new ProductItemFilterContents()
                {
                    Id = item.Id,
                    Images = ProductHelper.GetImages(item),
                    Price = ProductHelper.GetPrice(item)
                };
                result.Add(productItemFilterContent);
            }
            
        }
        return result;
    }

    private bool DoesColorMatch(ProductItem item, string color)
    {
        return item.VariantType switch
        {
            "Color" => item.Variants.ColorVariant.Any(co => co.Id == color),
            _ => item.Variants.SizeColorVariant.Any(co => co.Id == color)
        };
    }
        
}