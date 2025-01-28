using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Models;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Controllers;

[Route("product-item")]
public class ProductItemController(IMongoCollection<ProductItemDb> productItemCollection):ControllerBase
{
    // Add a new product item
    [HttpPost("add")]
    public async Task<IActionResult> AddProductItem([FromBody] ProductItemCommand productItem)
    {
        try
        {
            if (productItem == null) return BadRequest("ProductItem data is required.");
            
            // map product command to product item db
            //await productItemCollection.InsertOneAsync(productItem);
            return Ok(new { message = "Product item added successfully.", productItem = productItem });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding product item.", error = ex.Message });
        }
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllProductItems()
    {
        try
        {
            var products = await productItemCollection.Find(_ => true).ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    

}