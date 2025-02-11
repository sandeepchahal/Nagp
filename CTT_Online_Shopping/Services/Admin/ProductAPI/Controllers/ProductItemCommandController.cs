using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models.Commands;

namespace ProductAPI.Controllers;

public partial class ProductItemController
{
    // Add a new product item
    [HttpPost("add")]
    public async Task<IActionResult> AddProductItem([FromBody] ProductItemCommand productItem)
    {
        try
        {
            await productItemDbService.AddAsync(productItem);
            return Ok(new { message = "Product item added successfully.", productItem });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding product item.", error = ex.Message });
        }
    }
}