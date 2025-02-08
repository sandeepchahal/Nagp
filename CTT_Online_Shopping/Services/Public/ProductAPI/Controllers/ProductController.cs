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
    
    
    [HttpGet("get-all")] 
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var product = await productDbService.GetAll();
            return Ok(product);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
    
    

    [HttpGet("category/{slug}")]
    public async Task<IActionResult> GetBySubCategoryId(string slug)
    {
        try
        {
            var product = await productDbService.GetBySubCategorySlugAsync(slug);
            return Ok(product);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
    
}
