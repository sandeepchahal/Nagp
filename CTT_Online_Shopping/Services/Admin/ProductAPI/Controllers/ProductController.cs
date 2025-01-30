using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Events;
using ProductAPI.Models;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Controllers;

[Route("product")]
public partial class ProductController( IMongoCollection<ProductDb> productCollection, IProductEventService productEventService):ControllerBase
{
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
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetByProductId(string id)
    {
        try
        {
            var filter = Builders<ProductDb>.Filter.Eq(p => p.Id, id);
            var product = await productCollection.Find(filter).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound("No product found.");
            }
            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}