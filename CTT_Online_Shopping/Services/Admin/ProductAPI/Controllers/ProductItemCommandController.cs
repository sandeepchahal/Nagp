using Microsoft.AspNetCore.Mvc;
using ProductAPI.Helpers;
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
            if (string.IsNullOrEmpty(productItem.ProductId)) return BadRequest("product id is required");
            var productItemDb = ProductItemMapper.MapToDomainModel(productItem);
            await productItemCollection.InsertOneAsync(productItemDb);
            
            // send the event to update search db
           // var productItemAddEventModel = ProductItemMapper.MapToProductItemEvent(productItemDb);
           // _= productItemEventService.RaiseAddAsync(productItemAddEventModel);
            
            return Ok(new { message = "Product item added successfully.", productItem });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding product item.", error = ex.Message });
        }
    }
}