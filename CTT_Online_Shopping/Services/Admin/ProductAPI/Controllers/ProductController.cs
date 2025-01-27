using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Events;
using ProductAPI.Models;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Controllers;

[Route("product")]
public class ProductController( IMongoCollection<ProductDb> productCollection, IProductEvent productEvent):ControllerBase
{
    // Add a new product
    [HttpPost("add")]
    public async Task<IActionResult> AddProduct([FromBody] ProductCommand product)
    {
        try
        {
            if (product == null) return BadRequest("Product data is required.");
            ProductDb productDb = new()
            {
                Name = product.Name,
                Description = product.Description,
                Category = product.Category
            };
            await productCollection.InsertOneAsync(productDb);
            // send an event to search api
            _ = productEvent.RaiseAddProductAsync(productDb);
            return Ok(new { message = "Product added successfully.", product = productDb });
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