using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using SearchAPI.Models;

namespace SearchAPI.Controllers;

[Route("api/product-search")]
public class ProductSearchController(ElasticsearchClient elasticClient):ControllerBase
{
    [HttpPost("IndexProduct")]
    public async Task<IActionResult> IndexProduct([FromBody] Product product)
    {
        try
        {
            if (product == null) return BadRequest("Product data is required.");
        
            var response = await elasticClient.IndexAsync(product);
            if (!response.IsValidResponse)
                throw new Exception($"Failed to index product: {response.DebugInformation}");

            return Ok(new { message = "Product indexed successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error indexing product.", error = ex.Message });
        }
    }
    [HttpGet("search")]
    public async Task<IActionResult> SearchProduct([FromQuery] string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query)) return BadRequest("Search query is required.");

            var searchResponse = await elasticClient.SearchAsync<Product>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            sh => sh.Match(m => m
                                    .Field(f => f.Name) // Field to search for a match
                                    .Query(query) // Search term
                            ),
                            sh => sh.Match(m => m
                                    .Field(f => f.Category) // Also search the Category field
                                    .Query(query) // Search term
                            )
                        )
                    )
                )
            );

            if (!searchResponse.IsValidResponse)
                throw new Exception($"Search failed: {searchResponse.DebugInformation}");

            return Ok(searchResponse.Documents); // Returns matching products
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error performing search.", error = ex.Message });
        }
    }

}