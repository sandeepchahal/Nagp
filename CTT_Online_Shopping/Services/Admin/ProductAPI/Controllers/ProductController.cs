using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Models;

namespace ProductAPI.Controllers;

[Route("product")]
public class ProductController( IMongoCollection<Product> productCollection):ControllerBase
{
    // Add a new product
    [HttpPost("add")]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        try
        {
            if (product == null) return BadRequest("Product data is required.");

            await productCollection.InsertOneAsync(product);
            return Ok(new { message = "Product added successfully.", product });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding product.", error = ex.Message });
        }
    } 
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await productCollection.Find(_ => true).ToListAsync();
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