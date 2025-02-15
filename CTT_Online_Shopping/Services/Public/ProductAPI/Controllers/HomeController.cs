using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductAPI.DbServices;
using ProductAPI.Helper;
using ProductAPI.Models.Brands;
using ProductAPI.Models.Categories;
using ProductAPI.Models.Home;
using ProductAPI.Models.Products;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController(
    IMongoCollection<Product> productCollection,
    IMongoCollection<Category> categoryCollection,
    IBrandDbService brandDbService,
    IProductItemDbService productItemDbService):ControllerBase
{
    [HttpGet("get")]
    public async Task<IActionResult> Home()
    {
        try
        {
            var menCategory = await categoryCollection.Find(col => col.Gender == "Men").FirstOrDefaultAsync();
            var womenCategory = await categoryCollection.Find(col => col.Gender == "Women").FirstOrDefaultAsync();
            if(menCategory == null || womenCategory == null)
                return BadRequest("An error has occurred while processing the request. Please try again");
            
            var menPipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("categoryId", menCategory.Id)), // Filter by Category
                new BsonDocument("$sample", new BsonDocument("size", 5)) // Random selection
            };
            
            var womenPipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("categoryId", womenCategory.Id)), // Filter by Category
                new BsonDocument("$sample", new BsonDocument("size", 5)) // Random selection
            };

            var menProducts = await productCollection.Aggregate<Product>(menPipeline).ToListAsync();
            var womenProducts = await productCollection.Aggregate<Product>(womenPipeline).ToListAsync();

            var menResult = new List<ProductView>();

            foreach (var product in menProducts)
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

                menResult.Add(productView);
            }
            
            var womenResult = new List<ProductView>();

            foreach (var product in womenProducts)
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

                womenResult.Add(productView);
            }

            
            return Ok(new HomeQueryModel(){Men = menResult,Women = womenResult});
        }
        catch
        {
            return BadRequest("An error has occurred while processing the request. Please try again");
        }
    }
}