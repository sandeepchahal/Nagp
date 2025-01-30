using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using ProductConsumer.Models;

namespace ProductConsumer.DbServices;

public class ProductItemService(ElasticsearchClient elasticClient, ILogger<ProductItemService> logger)
    : IProductItemService
{
    public async Task Add(ProductItemEventModel productItemEvent)
    {
        try
        {
            // Step 1: Check if the product exists
            var productExists = await elasticClient.ExistsAsync<Product>(productItemEvent.ProductId);

            if (productExists.Exists)
            {
                await elasticClient.UpdateAsync<Product, object>(
                        productItemEvent.ProductId,
                        u => u
                            .Script(s => s
                                .Source("ctx._source.items.add(params.newItem)")
                                .Params(p => p.Add("newItem", new ProductItemEventModel()
                                {
                                    Id = productItemEvent.Id,
                                    Attributes = productItemEvent.Attributes,
                                    MinPrice = productItemEvent.MinPrice,
                                    MaxPrice = productItemEvent.MaxPrice,
                                    Name = productItemEvent.Name
                                }))
                            )
                    );
            }
        }
        catch (Exception e)
        {
            logger.LogError($"An error occurred while indexing the product item. Error - {e.Message}");
        }
    }

    public async Task Update(string id, ProductItemEventModel productItemEventModel)
    {
        try
        {
            var response = await elasticClient.UpdateAsync<ProductItemEventModel, object>(
                id,
                u => u
                    .Index("product-items")
                    .Doc(productItemEventModel)
                    .RetryOnConflict(3)
            );

            if (!response.IsValidResponse)
            {
                logger.LogError(
                    $"Failed to update the product item in Elasticsearch. Reason: {response.ElasticsearchServerError?.Error.Reason}");
            }
        }
        catch (Exception e)
        {
            logger.LogError($"An error occurred while updating the product item. Error: {e.Message}");
        }
    }

    private async Task<bool> IsProductItemExists(string productItemId)
    {
        var searchResponse = await elasticClient.SearchAsync<Product>(s => s
            .Query(q => q
                .Nested(n => n
                    .Path(nameof(Product.Items).ToLower())
                    .Query(nq => nq
                        .Term(t => t
                            .Field($"{nameof(Product.Items)}.{nameof(ProductItemEventModel.Id)}".ToLower())
                            .Value(productItemId)
                        )
                    )
                )
            )
        );
        return searchResponse.Documents.Any();
    }
}