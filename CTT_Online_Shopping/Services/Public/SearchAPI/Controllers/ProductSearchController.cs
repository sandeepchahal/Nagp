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
         // Process search results and generate suggestions
        var suggestions = new List<SuggestionResponse>();

        foreach (var hit in searchResponse.Hits)
        {
            var productItem = hit.Source;
            var highlights = hit.Highlight;
            var colorMatched = new List<string>();
            // Determine which fields matched
            var matchedFields = new List<string>();
            if (highlights?.ContainsKey("brand") == true)
                matchedFields.Add("brand");
            if (highlights?.ContainsKey("gender") == true)
                matchedFields.Add("gender");
            if (highlights?.ContainsKey("subCategoryName") == true)
                matchedFields.Add("subcategory");
            if (highlights?.ContainsKey("sizeColorVariant.color.color") == true)
                matchedFields.Add("color");

            // Construct base URL based on matched fields
            var urlBuilder = new StringBuilder("/search?");
            if (matchedFields.Contains("brand") && productItem?.Brand != null)
                urlBuilder.Append($"brand={productItem.Brand}&");
            if (matchedFields.Contains("gender") && productItem?.Gender != null)
                urlBuilder.Append($"gender={productItem.Gender}&");
            if (matchedFields.Contains("subcategory") && productItem?.SubCategoryId != null)
                urlBuilder.Append($"subcategory={productItem.SubCategoryId}&");

            // Handle color matching using highlights
            if (matchedFields.Contains("color") && productItem?.SizeColorVariant != null)
            {
                // Extract matched color names from highlights
                    colorMatched = highlights["sizeColorVariant.color.color"]
                    .Select(h => h.Replace("<em>", "").Replace("</em>", "")) // Remove highlighting tags
                    .Distinct()
                    .ToList();

                // Iterate through all nested documents and add matching colors
                foreach (var variant in productItem.SizeColorVariant)
                {
                    if (variant?.Color != null && colorMatched.Contains(variant.Color.Color, StringComparer.OrdinalIgnoreCase))
                    {
                        urlBuilder.Append($"color={variant.Color.ColorId}&");
                    }
                }
            }

            // Remove the trailing '&'
            var baseUrl = urlBuilder.ToString().TrimEnd('&');

            // Fetch subcategories based on matched fields
            var subcategories = FetchSubcategories(productItem, matchedFields, colorMatched);

            // Add suggestions
            suggestions.AddRange(from subcategory in subcategories
                from dic in subcategory
                select new SuggestionResponse
                {
                    Text = dic.Key, // Combined text
                    Value = $"/search?{dic.Value}" // Combined URL
                });
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
        
        private List<Dictionary<string,string>> FetchSubcategories(ProductItemEventModel productItem, List<string> matchedFields, List<string> matchedColor = null)
        {
            var subcategories = new List<Dictionary<string,string>>();
            
            // If gender is matched, add all subcategories for the gender
            if (matchedFields.Contains("gender") && productItem?.Gender != null)
            {
                var d = new Dictionary<string, string>();
                d.Add($"{productItem.Gender} {productItem.SubCategoryName}",
                    $"gender={productItem.Gender}&subcategory={productItem.SubCategoryId}");
                subcategories.Add(d);
            }

            // If brand is matched, add all subcategories for the brand and gender
            if (matchedFields.Contains("brand") && productItem?.Brand != null && productItem?.Gender != null)
            {
                var d = new Dictionary<string, string>();
                d.Add($"{productItem.Brand} {productItem.Gender} {productItem.SubCategoryName}",
                    $"brand={productItem.Brand}&gender={productItem.Gender}&subcategory={productItem.SubCategoryId}");
                subcategories.Add(d);
            }

            // If color is matched, add all subcategories for the color and gender
            if (matchedFields.Contains("color") && productItem?.SizeColorVariant != null && productItem?.Gender != null)
            {
                foreach (var variant in productItem.SizeColorVariant)
                {
                    if (matchedColor.Any() && matchedColor.Contains(variant.Color.Color))
                    {
                        var d = new Dictionary<string, string>();
                        d.Add($"{productItem.Gender} {variant.Color.Color} {productItem.SubCategoryName}",
                            $"gender={productItem.Gender}&color={variant.Color.ColorId}&subcategory={productItem.SubCategoryId}");
                        subcategories.Add(d);
                        
                        subcategories.Add(d);
                    }
                }
            }

            return subcategories.Distinct().ToList();
        }

    }
}