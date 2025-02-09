using Microsoft.AspNetCore.Mvc;
using ProductAPI.DbServices;

namespace ProductAPI.Controllers;

[Route("api/product/item")]
public class ProductItemController(IProductItemDbService productItemDbService):ControllerBase
{
    [HttpGet("get-by-product/{pid}")] 
    public async Task<IActionResult> GetProductById(string pid)
    {
        try
        {
            var product = await productItemDbService.GetByProductIdAsync(pid);
            return product is null? NotFound("Product id is not found") : Ok(product);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
    [HttpGet("get/{id}")] 
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var product = await productItemDbService.GetAsync(id);
            return product is null? NotFound("Product Item id is not found") : Ok(product);
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
            var product = await productItemDbService.GetAllAsync();
            return Ok(product);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
}