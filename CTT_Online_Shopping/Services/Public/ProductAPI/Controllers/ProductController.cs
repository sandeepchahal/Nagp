using Microsoft.AspNetCore.Mvc;
using ProductAPI.DbServices;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductDbService productDbService)
    : ControllerBase
{
    
    [HttpGet("get/{id}")] 
    public async Task<IActionResult> GetProductById(string id)
    {
        try
        {
            var product = await productDbService.GetAsync(id);
            return product is null? NotFound("Product id is not found") : Ok(product);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
    
}
