using System.Text;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
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
                                sh => sh.Match(m => m
                                    .Field(f => f.Gender)
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
                                    sh.Nested(nc => nc
                                        .Path("sizeColorVariant")
                                        .Query(cq =>
                                            cq.Match(m =>
                                                m.Field("sizeColorVariant.color.color")
                                                    .Query(query).Operator(Operator.And))
                                        )
                                    )
                            )
                        )
                    )
                    .Highlight(h => h
                        .Fields(f => f
                            // .Add("name", new HighlightFieldDescriptor<ProductItemEventModel>())
                            .Add("gender", new HighlightFieldDescriptor<ProductItemEventModel>())
                            .Add("subCategoryName", new HighlightFieldDescriptor<ProductItemEventModel>())
                            .Add("brand", new HighlightFieldDescriptor<ProductItemEventModel>())
                            .Add("sizeColorVariant.color.color", new HighlightFieldDescriptor<ProductItemEventModel>()
                            )
                        )
                    )
                );

                if (!searchResponse.IsValidResponse)
                    throw new Exception($"Search failed: {searchResponse.DebugInformation}");
                // Process search results and generate suggestions
                var suggestions = new List<SuggestionResponse>();

                foreach (var hit in searchResponse.Hits)
                {
                    var productItem = hit.Source;
                    var highlights = hit.Highlight;

                    // Determine which fields matched
                    var matchedFields = new List<string>();
                    if (highlights?.ContainsKey("gender") == true)
                        matchedFields.Add("gender");
                    if (highlights?.ContainsKey("subCategoryName") == true)
                        matchedFields.Add("subcategory");
                    if (highlights?.ContainsKey("brand") == true)
                        matchedFields.Add("brand");
                    if (highlights?.ContainsKey("sizeColorVariant.color.color") == true)
                        matchedFields.Add("color");

                    // Construct combined URL based on matched fields
                    var urlBuilder = new StringBuilder("/search?");
                    if (matchedFields.Contains("brand") && productItem?.Brand != null)
                        urlBuilder.Append($"brand={productItem.Brand}&");
                    if (matchedFields.Contains("gender") && productItem?.Brand != null)
                        urlBuilder.Append($"gender={productItem.Gender}&");
                    if (matchedFields.Contains("subcategory") && productItem?.SubCategoryId != null)
                        urlBuilder.Append($"subcategory={productItem.SubCategoryId}&");
                    
                    
                    if (matchedFields.Contains("color") && productItem?.SizeColorVariant != null)
                    {
                        // Extract matched color names from highlights
                        var matchedColors = highlights["sizeColorVariant.color.color"]
                            .Select(h => h.Replace("<em>", "").Replace("</em>", "")) // Remove highlighting tags
                            .Distinct()
                            .ToList();

                        // Iterate through all nested documents and add matching colors
                        foreach (var variant in productItem.SizeColorVariant)
                        {
                            if (variant?.Color != null && matchedColors.Contains(variant.Color.Color, StringComparer.OrdinalIgnoreCase))
                            {
                                urlBuilder.Append($"color={variant.Color.ColorId}&");
                            }
                        }
                    }

                    // Remove the trailing '&'
                    var url = urlBuilder.ToString().TrimEnd('&');

                    // Add individual suggestions for each matched field
                    if (matchedFields.Contains("brand"))
                    {
                        suggestions.Add(new SuggestionResponse
                        {
                            Text = productItem?.Brand, // Full text of the brand
                            Value = $"/search?brand={productItem?.Brand}" // URL for brand
                        });
                    }
                    if (matchedFields.Contains("gender"))
                    {
                        suggestions.Add(new SuggestionResponse
                        {
                            Text = productItem?.Gender, // Full text of the subcategory
                            Value = $"/search?gender={productItem?.Gender}" // URL for subcategory
                        });
                    }
                    if (matchedFields.Contains("subcategory"))
                    {
                        suggestions.Add(new SuggestionResponse
                        {
                            Text = productItem?.SubCategoryName, // Full text of the subcategory
                            Value = $"/search?subcategory={productItem?.SubCategoryId}" // URL for subcategory
                        });
                    }
                    if (matchedFields.Contains("color"))
                    {
                        // Iterate through all matching nested documents
                        foreach (var variant in productItem?.SizeColorVariant ?? Enumerable.Empty<ProductVariantSizeAndColorEventModel>())
                        {
                            if (variant?.Color != null && highlights?["sizeColorVariant.color.color"]?.Any(h => h.Contains(variant.Color.Color, StringComparison.OrdinalIgnoreCase)) == true)
                            {
                                suggestions.Add(new SuggestionResponse
                                {
                                    Text = variant.Color.Color, // Full text of the color
                                    Value = $"/search?color={variant.Color.ColorId}" // URL for color
                                });
                            }
                        }
                    }

                    // Add combined suggestion if multiple fields matched
                    if (matchedFields.Count > 1)
                    {
                        suggestions.Add(new SuggestionResponse
                        {
                            Text = string.Join(" ", matchedFields.Select(f =>
                            {
                                return f switch
                                {
                                    "brand" => productItem?.Brand,
                                    "gender" => productItem?.Gender,
                                    "subcategory" => productItem?.SubCategoryName,
                                    "color" => productItem?.SizeColorVariant
                                        ?.FirstOrDefault(v => highlights?["sizeColorVariant.color.color"]?.Any(h => h.Contains(v?.Color?.Color, StringComparison.OrdinalIgnoreCase)) == true)
                                        ?.Color?.Color,
                                    _ => null
                                };
                            }).Where(t => t != null)), // Combined text
                            Value = url // Combined URL
                        });
                    }
                }

                // Remove duplicate suggestions (if any)
                var uniqueSuggestions = suggestions
                    .GroupBy(s => s.Text) // Group by text
                    .Select(g => g.First()) // Take the first suggestion in each group
                    .ToList();

                return Ok(uniqueSuggestions);
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