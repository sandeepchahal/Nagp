using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Controllers;

public partial class ProductController
{
    // Add a new product
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] ProductCommand product)
    {
        try
        {
            if (product.CategoryId == null || product.SubCategoryId == null) return BadRequest("Product data is required.");
            ProductDb productDb = new()
            {
                Name = product.Name,
                Description = product.Description,
                SubCategoryId = product.SubCategoryId,
                CategoryId = product.CategoryId,
                Brand = product.Brand
            };
            await productCollection.InsertOneAsync(productDb);
            // send an event to search api
            _ = productEventService.RaiseAddAsync(productDb);
            return Ok(new { message = "Product added successfully.", product = productDb });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding product.", error = ex.Message });
        }
    }
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductCommand productCommand)
    {
        try
        {
            var filter = Builders<ProductDb>.Filter.Eq(p => p.Id, id);
            var update = Builders<ProductDb>.Update
                .Set(p => p.Description, productCommand.Description)
                .Set(p => p.CategoryId, productCommand.CategoryId)
                .Set(p => p.SubCategoryId, productCommand.SubCategoryId)
                .Set(p => p.Name, productCommand.Name);

            await productCollection.UpdateOneAsync(filter, update);// return number of rows updated
            var updatedProduct = await productCollection.Find(filter).FirstOrDefaultAsync();

            // send an event to search api
            _ = productEventService.RaiseUpdateAsync(updatedProduct);
            return Ok(new { message = "Product added successfully.", product = updatedProduct });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding product.", error = ex.Message });
        }
    }
}