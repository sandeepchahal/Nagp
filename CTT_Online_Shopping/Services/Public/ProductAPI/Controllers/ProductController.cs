using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Models;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(
    IMongoCollection<Product> productCollection,
    IMongoCollection<ProductItem> productItemCollection)
    : ControllerBase
{
    // Add a new product
    [HttpPost("AddProduct")]
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

    // Add a new product item
    [HttpPost("AddProductItem")]
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
    [HttpGet("get-all-items")]
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
