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

                var searchResponse = await _elasticClient.SearchAsync<ProductItemEventModel>(s => s
                    .Query(q => q
                        .Bool(b => b
                            .Should
                            (
                                // Match against key fields
                                sh => sh.Match(m => m
                                    .Field(f => f.Name)
                                    .Query(query)
                                    .Fuzziness(new Fuzziness("Auto"))
                                ),
                                sh => sh.Match(m => m
                                    .Field(f => f.SubCategoryName)
                                    .Query(query)
                                    .Fuzziness(new Fuzziness("Auto"))
                                ),
                                sh => sh.Match(m => m
                                    .Field(f => f.Brand)
                                    .Query(query)
                                    .Fuzziness(new Fuzziness("Auto"))
                                ),
                                sh =>
                                    sh.Nested(n => n
                                        .Path(p => p.SizeColorVariant)
                                        .Query(nq =>
                                            nq.Match(mt => mt
                                                .Field(f => f.SizeColorVariant.First().Color)
                                                .Query(query)
                                                .Fuzziness(new Fuzziness("Auto"))
                                            )
                                        )
                                    )
                            )
                        )
                    )
                    .Highlight(h => h
                        .Fields(f => f
                            .Add("name", new HighlightFieldDescriptor<ProductItemEventModel>())
                            .Add("subCategoryName", new HighlightFieldDescriptor<ProductItemEventModel>())
                            .Add("brand", new HighlightFieldDescriptor<ProductItemEventModel>())
                            .Add("sizeColorVariant.color", new HighlightFieldDescriptor<ProductItemEventModel>())
                            .Add("color", new HighlightFieldDescriptor<ProductItemEventModel>())
                            .Add("sizeColorVariant.first().color",
                                new HighlightFieldDescriptor<ProductItemEventModel>())
                        )
                    )
                );

                if (!searchResponse.IsValidResponse)
                    throw new Exception($"Search failed: {searchResponse.DebugInformation}");

                // Return search results with highlights (if any)
                var results = searchResponse.Hits.Select(hit => new
                {
                    ProductItem = hit.Source, // The product item document
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