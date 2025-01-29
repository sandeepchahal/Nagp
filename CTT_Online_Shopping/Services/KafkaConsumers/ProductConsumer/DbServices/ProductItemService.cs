using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using ProductConsumer.Models;

namespace ProductConsumer.DbServices;

public class ProductItemService(ElasticsearchClient elasticClient, ILogger<ProductItemService> logger):IProductItemService
{
    public async Task Add(ProductItem productItem)
    {
        try
        {
            await elasticClient
                .IndexAsync(productItem, 
                    idx => idx.Index("product-items"));
        }
        catch (Exception e)
        {
            logger.LogError($"An error occurred while indexing the product item. Error - {e.Message}");
        }
    }

    public async Task Update(string id, ProductItem productItem)
    {
        try
        {
            var response = await elasticClient.UpdateAsync<ProductItem, object>(
                id,
                u => u
                    .Index("product-items")
                    .Doc(productItem)
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
}