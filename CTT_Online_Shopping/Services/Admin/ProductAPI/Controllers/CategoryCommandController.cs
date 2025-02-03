using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Models.Abstract;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Controllers;

public partial class CategoryController
{
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] CategoryCommand category)
    {
        try
        {
            // Check if the main category already exists
            var existingCategory = await categoryCollection
                .Find(c => c.MainCategory.Equals(category.MainCategory, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync();

            if (existingCategory != null)
            {
                return BadRequest(new { message = "Main category already exists." });
            }

            var newCategory = new CategoryDb()
            {
                Gender = category.Gender,
                MainCategory = category.MainCategory,
                SubCategories = category.SubCategories.Select(sub =>
                    new SubCategoryDb(sub.Name, nameof(category.Gender), 
                        sub.FilterAttributes.Select(att=>
                            new FilterAttributeDb(){Name = att.Name,Options = att.Options, Type = att.Type}).ToList())).ToList()
            };

            await categoryCollection.InsertOneAsync(newCategory);

            return CreatedAtAction(nameof(Add), new { id = newCategory.Id }, newCategory);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding product.", error = ex.Message });
        }
    }
}