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
            // Check if productItemId already exists
            var existingProduct = await elasticClient.SearchAsync<ProductItemEventModel>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Must(
                            mq => mq.Term(t => t.Field(f => f.ProductItemId).Value(productItemEvent.ProductItemId)),
                            mq => mq.Term(t => t.Field(f => f.ProductId).Value(productItemEvent.ProductId))
                        )
                    )
                )
            );

            if (existingProduct.Documents.Any())
            {
                logger.LogInformation($"Skipping indexing. Product Item with ID {productItemEvent.ProductItemId} already exists.");
                return; // Skip indexing if it already exists
            }

            // Index new document
            var indexResponse = await elasticClient.IndexAsync(productItemEvent, i => i
                    .Index("your-index-name") // Replace with your actual index name
            );

            if (indexResponse.IsValidResponse)
            {
                logger.LogInformation($"Product Item with ID {productItemEvent.ProductItemId} successfully indexed.");
            }
            else
            {
                logger.LogWarning($"Failed to index Product Item with ID {productItemEvent.ProductItemId}. Response: {indexResponse.DebugInformation}");
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
        var searchResponse = await elasticClient.SearchAsync<ProductItemEventModel>(s => s
            .Query(q => q
                .Term(t => t
                    .Field(f => f.ProductItemId) // Directly check the ProductItemId field
                    .Value(productItemId)
                )
            )
        );
        return searchResponse.Documents.Any();
    }

}