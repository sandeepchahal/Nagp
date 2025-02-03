using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Microsoft.AspNetCore.Mvc;
using SearchAPI.Models;

namespace SearchAPI.Controllers
{
    [Route("api/search")]
    public class ProductSearchController : ControllerBase
    {
        private readonly ElasticsearchClient _elasticClient;

        public ProductSearchController(ElasticsearchClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

       [HttpGet("text")]
public async Task<IActionResult> SearchProduct([FromQuery] string query)
{
    try
    {
        if (string.IsNullOrWhiteSpace(query)) return BadRequest("Search query is required.");

        var searchResponse = await _elasticClient.SearchAsync<Product>(s => s
            .Query(q => q
                .Bool(b => b
                    .Should(
                        // Match against top-level fields (Name, Category, etc.)
                        sh => sh.Match(m => m
                                .Field(f => f.Name) // Search in the Name field
                                .Query(query)
                                .Fuzziness(new Fuzziness("Auto")) // Enable fuzzy search for partial matches
                        ),
                        sh => sh.Match(m => m
                                .Field(f => f.Category) // Search in the Category field
                                .Query(query)
                                .Fuzziness(new Fuzziness("Auto")) // Enable fuzzy search for partial matches
                        ),
                        sh => sh.Match(m => m
                                .Field(f => f.Brand) // Search in the Category field
                                .Query(query)
                                .Fuzziness(new Fuzziness("Auto")) // Enable fuzzy search for partial matches
                        ),
                        sh => sh.Match(m => m
                                .Field(f => f.Id) // Search in the Category field
                                .Query(query)
                                .Fuzziness(new Fuzziness("Auto")) // Enable fuzzy search for partial matches
                        ),
                        // Match against nested fields (items.attributes)
                        sh => sh.Nested(n => n
                            .Path(p => p.Items) // Path to the nested object 'Items'
                            .Query(nq => nq
                                .Bool(bq => bq
                                    .Should(
                                        // Match against attributes in items (nested)
                                        m => m.Match(mt => mt
                                            .Field(f => f.Items.First().Attributes) // Search in the attributes field of Items
                                            .Query(query)
                                            .Fuzziness(new Fuzziness("Auto")) // Enable fuzzy search for partial matches
                                        ),
                                        // Optionally, match other fields in items (e.g., Name, ProductId)
                                        m => m.Match(mt => mt
                                            .Field(f => f.Items.First().Name) // Search in the Name field of Items
                                            .Query(query)
                                            .Fuzziness(new Fuzziness("Auto"))
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            )
            .Highlight(h => h
                .Fields(f => f
                    .Add("name", new HighlightFieldDescriptor<Product>())
                    .Add("category", new HighlightFieldDescriptor<Product>())
                    .Add("items.attributes", new HighlightFieldDescriptor<Product>())
                )
            )
        );

        if (!searchResponse.IsValidResponse)
            throw new Exception($"Search failed: {searchResponse.DebugInformation}");

        // Return search results with highlights (if enabled)
        var results = searchResponse.Hits.Select(hit => new
        {
            Product = hit.Source, // The product document
            Highlights = hit.Highlight // Highlighted terms (if any)
        });

        return Ok(results);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "Error performing search.", error = ex.Message });
    }
}


        [HttpGet("get-mapping")]
        public async Task<IActionResult> GetMapping()
        {
            var mappingResponse = await _elasticClient.Indices.GetMappingAsync("products");

            if (mappingResponse.IsValidResponse)
            {
                var mappings = mappingResponse.Indices["products"].Mappings;
                return Ok(mappings.Properties);
            }
            else
            {
                return BadRequest("");
            }
        }
    }
}