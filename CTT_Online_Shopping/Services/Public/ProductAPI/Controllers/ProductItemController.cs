using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.DbServices;
using ProductAPI.Helper;
using ProductAPI.Models.Brands;
using ProductAPI.Models.Products;

namespace ProductAPI.Controllers;

[EnableCors("AllowAll")]
[Route("api/product/item")]
public class ProductItemController(
    IProductItemDbService productItemDbService,
    IMongoCollection<Product> productCollection, 
    IBrandDbService brandDbService):ControllerBase
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
            productItem.Product = productView;
            
            productItem.Product.Images = ProductHelper.GetImages(productItem!);
            productItem.Product.Price = ProductHelper.GetPrice(productItem!);
            productItem.Product.ProductItemId = productItem.Id;
            
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