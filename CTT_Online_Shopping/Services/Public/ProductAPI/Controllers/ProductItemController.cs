using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.DbServices;
using ProductAPI.Helper;
using ProductAPI.Models.Brands;
using ProductAPI.Models.Products;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/product/item")]
public class ProductItemController(
    IProductItemDbService productItemDbService,
    IMongoCollection<Product> productCollection, 
    IBrandDbService brandDbService, IReviewDbService reviewDbService):ControllerBase
{
    [HttpGet("get-by-product/{pid}")] 
    public async Task<IActionResult> GetProductById(string pid)
    {
        try
        {
            var product = await productItemDbService.GetByProductIdAsync(pid);
            return product is null? NotFound("Product id is not found") : Ok(product);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
    [HttpGet("get/{id}")] 
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var productItem = await productItemDbService.GetAsync(id);
            if (productItem is null)
            {
                return NotFound("Product is not found");
            }
            var product = await productCollection.Find(p =>p.Id == productItem.ProductId).FirstOrDefaultAsync();
            var productView = ProductHelper.MapToProductView(product);
            productView.Brand = await brandDbService.GetAsync(product.BrandId) ?? new Brand();
            productView.Reviews = await reviewDbService.GetByProductId(id);
            productItem.Product = productView;
            productItem.Product.Images = ProductHelper.GetImages(productItem!);
            productItem.Product.Price = ProductHelper.GetPrice(productItem!);
            productItem.Product.ProductItemId = productItem.Id;
            
            var similarBrand = await productItemDbService.GetByBrand(product.BrandId);
            var excludeCurrentItem = 
                similarBrand.Products.Where(col => col.ProductItemId != id).ToList();

            productItem.SimilarBrand = similarBrand.Brand != null
                ? new ProductWithSimilarBrand()
                    { Brand = similarBrand.Brand, Products = excludeCurrentItem }
                : null;
            
            var similarSubcategory = await productItemDbService.GetBySubcategoryType(product.SubCategoryId);
            
            var excludeSimilarSubcategory = similarSubcategory.SubCategory != null?
                similarSubcategory.Products.Where(col => col.ProductItemId != id).ToList():null;

            productItem.SimilarSubCategory = new ProductWithSimilarChoiceView()
                { SubCategory = similarSubcategory.SubCategory??null, Products = excludeSimilarSubcategory };
            
            var similarCategory = await productItemDbService.GetByCategoryType(product.CategoryId);
            
            var excludeSimilarCategory = similarCategory.Category !=null?
                similarCategory.Products.Where(col => col.ProductItemId != id).ToList():null;

            productItem.SimilarCategory = new ProductWithSimilarGenderView()
                { Category = similarCategory.Category??null, Products = excludeSimilarCategory };
            
            return Ok(productItem);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
    [HttpGet("get-all")] 
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var product = await productItemDbService.GetAllAsync();
            return Ok(product);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
}