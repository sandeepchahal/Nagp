using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Controllers;

[Route("api/category")]
public partial class CategoryController(IMongoCollection<CategoryDb> categoryCollection):ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var categories = await categoryCollection.Find(_ => true).ToListAsync();
            if (categories == null || categories.Count == 0)
            {
                return NotFound("No categories found.");
            }
            return Ok(categories);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var category = await categoryCollection.Find(col=>col.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound($"No categories found for id {id}");
            }
            return Ok(category);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }
}