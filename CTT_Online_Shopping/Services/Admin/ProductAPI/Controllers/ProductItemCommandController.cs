using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Controllers;

public partial class ProductItemController
{
    // Add a new product item
    [HttpPost("add")]
    public async Task<IActionResult> AddProductItem([FromBody] ProductItemCommand productItem)
    {
        try
        {
            if (string.IsNullOrEmpty(productItem.ProductId)) return BadRequest("Product Id is required.");
            
            var productItemDb = new ProductItemDb()
            {
                Attributes = productItem.Attributes,
                Price = productItem.Price,
                Quantity = productItem.Quantity,
                Sku = productItem.Sku,
                ProductId = productItem.ProductId
            };
            await productItemCollection.InsertOneAsync(productItemDb);
            
            // send the event to update search db
            _= productItemEvent.RaiseAddAsync(productItemDb);
            
            return Ok(new { message = "Product item added successfully.", productItem = productItem });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding product item.", error = ex.Message });
        }
    }
}