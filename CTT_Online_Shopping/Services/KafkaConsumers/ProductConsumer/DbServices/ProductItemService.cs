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
            // Step 1: Insert the ProductItemEventModel directly into the database (Elasticsearch)
            var indexResponse = await elasticClient.IndexAsync(productItemEvent);

            // Optional: You can log the successful insert
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