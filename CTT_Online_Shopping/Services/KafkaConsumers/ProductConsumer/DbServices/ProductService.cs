using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using ProductConsumer.Models;

namespace ProductConsumer.DbServices;

public class ProductService(ElasticsearchClient elasticClient, ILogger<ProductService> logger):IProductService
{
    
    public async Task Add(Product product)
    {
        try
        {
            await elasticClient
            .IndexAsync(product, idx => idx.Index("products"));
        }
        catch (Exception e)
        {
            logger.LogError($"An error occurred while indexing the product \n Error - {e.Message}");
        }
    }

    public async Task Update(Product product)
    {
       
    }
}