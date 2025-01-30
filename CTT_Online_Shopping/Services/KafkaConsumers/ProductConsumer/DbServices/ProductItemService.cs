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
                // Step 2: Check if the product item already exists
                var productItemExists = await IsProductItemExists(productItemEvent.Id);

                if (productItemExists)
                {
                    // Update the existing product item
                    await elasticClient.UpdateAsync<Product, object>(
                        productItemEvent.ProductId,
                        u => u
                            .Script(s => s
                                .Source(@"
                            for (int i = 0; i < ctx._source.items.length; i++) {
                                if (ctx._source.items[i].productItemId == params.newItem.productItemId) {
                                    ctx._source.items[i] = params.newItem;
                                    break;
                                }
                            }
                        ")
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
                else
                {
                    // Add the new product item to the items array
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
            else
            {
                // Create a new product document with the product item
                var product = new Product
                {
                    Id = productItemEvent.ProductId,
                    Name = "Unknown", // Default name (can be updated later)
                    Category = "Unknown", // Default category (can be updated later)
                    Description = "Unknown", // Default description (can be updated later)
                    Items = new List<ProductItemEventModel>
                    {
                        new ProductItemEventModel()
                        {
                            Id = productItemEvent.Id,
                            Attributes = productItemEvent.Attributes,
                            MinPrice = productItemEvent.MinPrice,
                            MaxPrice = productItemEvent.MaxPrice,
                            Name = productItemEvent.Name
                        }
                    }
                };

                // Index the product in Elasticsearch
                await elasticClient.IndexAsync(product, idx => idx.Index("products"));
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