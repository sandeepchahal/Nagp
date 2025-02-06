using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.DbServices;
using ProductAPI.Events;
using ProductAPI.Helpers;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.Controllers;

[Route("api/product")]
public partial class ProductController(
    IMongoCollection<ProductDb> productCollection,
    IProductEventService productEventService,
    ICategoryDbService categoryDbService) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await productCollection.Find(_ => true).ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound("No products found.");
            }

            var productViewList = new List<ProductView>();
            foreach (var productDb in products)
            {
                var mapped = ProductMapper.MapToProductView(productDb);
                var category = await categoryDbService.GetAsync(productDb.CategoryId);
                mapped.Category = new ProductCategoryView()
                {
                    Id = category!.Id,
                    Gender = category.Gender,
                    Name = category.Name,
                    SubCategory = category.SubCategories.Where(col => col.Id == productDb.SubCategoryId).Select(col =>
                        new SubCategoryView()
                        {
                            Id = col.Id,
                            Name = col.Name
                        }).FirstOrDefault() ?? new SubCategoryView()
                };
                productViewList.Add(mapped);
            }

            return Ok(productViewList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetByProductId(string id)
    {
        try
        {
            var filter = Builders<ProductDb>.Filter.Eq(p => p.Id, id);
            var product = await productCollection.Find(filter).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound("No product found.");
            }

            var mapped = ProductMapper.MapToProductView(product);
            var category = await categoryDbService.GetAsync(product.CategoryId);
            mapped.Category = new ProductCategoryView()
            {
                Id = category!.Id,
                Gender = category.Gender,
                Name = category.Name,
                
                SubCategory = category.SubCategories.Where(col => col.Id == product.SubCategoryId).Select(col =>
                    new SubCategoryView()
                    {
                        Id = col.Id,
                        Name = col.Name
                    }).FirstOrDefault() ?? new SubCategoryView()
            };
            return Ok(mapped);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}