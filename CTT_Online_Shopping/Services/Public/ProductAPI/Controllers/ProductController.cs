using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Models;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(
    IMongoCollection<Product> productCollection)
    : ControllerBase
{
    // Read product by ID
    [HttpGet("get/{id}")] 
    public async Task<IActionResult> GetProductById(string id)
    {
        try
        {
            var product = await productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null) return NotFound(new { message = "Product not found." });

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error fetching product.", error = ex.Message });
        }
    }
    
}
