using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.DbServices;
using ProductAPI.Events;
using ProductAPI.Helpers;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.Controllers;

[Route("api/product/item")]
public partial class ProductItemController(
    IMongoCollection<ProductItemDb> productItemCollection,
    IProductItemEventService productItemEventService,
    IProductDbService productDbService):ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllProductItems()
    {
        try
        {
            var productItems = await productItemCollection.Find(_ => true).ToListAsync();
            if (productItems == null || productItems.Count == 0)
            {
                return NotFound("No products found.");
            }

            var result = new List<ProductItemView>();
            foreach (var mapper in productItems.Select(productItem => ProductItemMapper.MapToProductViewModel(productItem)))
            {
                mapper.Product =await productDbService.GetAsync(mapper.ProductId)?? new ProductView();
                result.Add(mapper);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    

}