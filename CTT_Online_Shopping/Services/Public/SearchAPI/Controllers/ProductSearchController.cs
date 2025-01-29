using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using SearchAPI.Models;

namespace SearchAPI.Controllers;

[Route("api/product-search")]
public class ProductSearchController(ElasticsearchClient elasticClient):ControllerBase
{
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