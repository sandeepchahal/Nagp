using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DbServices;
using ProductAPI.Models.Order;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController(
    IProductDbService productDbService,
    ILogger<ProductController> logger, 
    ICategoryDbService categoryDbService,
    IOrderDbService orderDbService)
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
    
    
    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            logger.LogInformation("Category is hit successfully");
            var categories = await categoryDbService.GetAllCategories();
            logger.LogInformation($"Categories  count - {categories.Count}");
            return Ok(categories);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? gender = null, 
        [FromQuery] string? brand = null, 
        [FromQuery] string? color = null,
        [FromQuery] string? subcategory = null)
    {
        var result =
            await productDbService.GetAsync(gender: gender, brand: brand, color: color, subcategory: subcategory);
        
        return Ok(result);
    }


    [HttpPost("order")]
    public async Task<IActionResult> CreateOrder(OrderRequest request)
    {
        try
        {
            // create an order
            var result  =await orderDbService.CreateOrderAsync(request: request);
            // update user details in user service
            
            return Ok(result);
        }
        catch
        {
            return BadRequest("An error has occurred");
        }
    }

    [HttpGet("order/get")]
    public async Task<IActionResult> GetOrderlist()
    {
        try
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Missing or invalid token");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Invalid token");
            }

            var result = await orderDbService.GetOrdersAsync(email);
            return Ok(result);
        }
        catch
        {
            return BadRequest("An error has occurred");
        }
    }
    
    [HttpGet("order/get/{id}")]
    public async Task<IActionResult> GetOrderlist(string id)
    {
        try
        {
            
            var result = await orderDbService.GetAsync(id);
            return Ok(result);
        }
        catch
        {
            return BadRequest("An error has occurred");
        }
    }

}
