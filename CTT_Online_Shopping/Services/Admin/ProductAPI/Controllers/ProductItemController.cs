using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Events;
using ProductAPI.Models;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Controllers;

[Route("api/product/item")]
public partial class ProductItemController(IMongoCollection<ProductItemDb> productItemCollection, IProductItemEventService productItemEventService):ControllerBase
{
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