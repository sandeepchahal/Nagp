using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Models;

namespace ProductAPI.Controllers;

[Route("product-item")]
public class ProductItemController(IMongoCollection<ProductItem> productItemCollection):ControllerBase
{
    // Add a new product item
    [HttpPost("add")]
    public async Task<IActionResult> AddProductItem([FromBody] ProductItem productItem)
    {
        try
        {
            if (productItem == null) return BadRequest("ProductItem data is required.");

            await productItemCollection.InsertOneAsync(productItem);
            return Ok(new { message = "Product item added successfully.", productItem });
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