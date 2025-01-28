using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using ProductConsumer.Models;

namespace ProductConsumer.DbServices;

public class ProductService(ElasticsearchClient elasticClient, ILogger<ProductService> logger) : IProductService
{
    public async Task Add(Product product)
    {
        try
        {
            await elasticClient
                .IndexAsync(product,
                    idx => idx.Index("products"));
        }
        catch (Exception e)
        {
            logger.LogError($"An error occurred while indexing the product \n Error - {e.Message}");
        }
    }

    public async Task Update(string id, Product product)
    {
        try
        {
            // Update the document in Elasticsearch
            var response = await elasticClient.UpdateAsync<Product, object>(
                id, // Pass the document ID
                u => u
                    .Index("products") // Specify the index name
                    .Doc(product) // Pass the partial document (or full document for update)
                    .RetryOnConflict(3) // Optional: Retry in case of version conflicts
            );

            // Check if the update was successful
            if (!response.IsValidResponse)
            {
                logger.LogError(
                    $"Failed to update the product in Elasticsearch. Reason: {response.ElasticsearchServerError?.Error?.Reason}");
            }
        }
        catch (Exception e)
        {
            // Log any unexpected exceptions
            logger.LogError($"An error occurred while updating the product. Error: {e.Message}");
        }
    }
}