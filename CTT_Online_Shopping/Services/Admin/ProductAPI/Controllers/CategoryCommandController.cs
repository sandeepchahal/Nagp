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
                .Find(c => c.MainCategory.Equals(category.MainCategory, StringComparison.OrdinalIgnoreCase)
                           && c.Gender.Equals(category.Gender, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync();

            if (existingCategory != null)
            {
                // Iterate over each subcategory
                foreach (var sub in category.SubCategories)
                {
                    var existingSubCategory = existingCategory.SubCategories
                        .FirstOrDefault(s => s.Name.Equals(sub.Name, StringComparison.OrdinalIgnoreCase));

                    if (existingSubCategory == null) // Add new subcategory
                    {
                        existingCategory.SubCategories.Add(new SubCategoryDb()
                        {
                            Name = sub.Name,
                            Slug = string.IsNullOrEmpty(sub.Slug) ? GenerateSlug(category.Gender, sub.Name) : sub.Slug,
                            FilterAttributes = sub.FilterAttributes.Select(att =>
                                    new FilterAttributeDb() { Name = att.Name, Options = att.Options, Type = att.Type })
                                .ToList()
                        });
                    }
                    else
                    {
                        // Update only missing filter attributes
                        foreach (var att in sub.FilterAttributes)
                        {
                            var existingAttribute = existingSubCategory.FilterAttributes
                                .FirstOrDefault(a => a.Name.Equals(att.Name, StringComparison.OrdinalIgnoreCase));

                            if (existingAttribute == null)
                            {
                                existingSubCategory.FilterAttributes.Add(new FilterAttributeDb()
                                {
                                    Name = att.Name,
                                    Options = att.Options,
                                    Type = att.Type
                                });
                            }
                        }
                    }
                }

                // Update the existing category in the database
                await categoryCollection.ReplaceOneAsync(c => c.Id == existingCategory.Id, existingCategory);

                return Ok(new { message = "Subcategories updated successfully." });
            }

            // If category does not exist, create a new one
            var newCategory = new CategoryDb()
            {
                Gender = category.Gender,
                MainCategory = category.MainCategory,
                SubCategories = category.SubCategories.Select(sub =>
                    new SubCategoryDb()
                    {
                        Name = sub.Name,
                        Slug = string.IsNullOrEmpty(sub.Slug) ? GenerateSlug(category.Gender, sub.Name) : sub.Slug,
                        FilterAttributes = sub.FilterAttributes.Select(att =>
                                new FilterAttributeDb() { Name = att.Name, Options = att.Options, Type = att.Type })
                            .ToList()
                    }).ToList()
            };

            await categoryCollection.InsertOneAsync(newCategory);

            return CreatedAtAction(nameof(Add), new { id = newCategory.Id }, newCategory);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding/updating category.", error = ex.Message });
        }
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateCategory(string id, [FromBody] CategoryCommand category)
    {
        try
        {
            // Check if the category exists
            var existingCategory = await categoryCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (existingCategory == null)
            {
                return NotFound(new { message = "Category not found." });
            }

            // Update the category fields
            var updatedCategory = Builders<CategoryDb>.Update
                .Set(c => c.Gender, category.Gender)
                .Set(c => c.MainCategory, category.MainCategory)
                .Set(c => c.SubCategories, category.SubCategories.Select(sub =>
                        new SubCategoryDb
                        {
                            Name = sub.Name,
                            Slug = string.IsNullOrEmpty(sub.Slug) ? GenerateSlug(category.Gender, sub.Name) : sub.Slug,
                            FilterAttributes = sub.FilterAttributes.Select(att =>
                                new FilterAttributeDb
                                {
                                    Name = att.Name,
                                    Options = att.Options,
                                    Type = att.Type
                                }).ToList()
                        }).ToList()
                );

            // Apply the update in MongoDB
            var result = await categoryCollection.UpdateOneAsync(
                c => c.Id == id,
                updatedCategory
            );

            if (result.ModifiedCount == 0)
            {
                return BadRequest(new { message = "No changes detected or update failed." });
            }

            return Ok(new { message = "Category updated successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating category.", error = ex.Message });
        }
    }

    private string GenerateSlug(string gender, string name)
    {
        return $"{gender}-{name}"
            .ToLower()
            .Replace(" ", "-")
            .Replace("&", "and")
            .Replace("/", "-");
    }
}