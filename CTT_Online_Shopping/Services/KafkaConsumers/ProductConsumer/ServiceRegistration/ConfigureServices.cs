using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductConsumer.DbServices;
using ProductConsumer.Models;
using ProductConsumer.Services;

namespace ProductConsumer.ServiceRegistration;

public static class ConfigureServices
{
    public static void RegisterBackgroundServices(this IServiceCollection serviceCollection)
    {
        // Register background service
        serviceCollection.AddSingleton<IHostedService, ProductConsumerService>();
        serviceCollection.AddSingleton<IHostedService, ProductItemConsumerService>();
    }

    public static async Task RegisterElasticClientServices(this IServiceCollection serviceCollection,
        ConfigurationManager configurationManager)
    {
        serviceCollection.AddSingleton<ElasticsearchClient>(_ =>
        {
            var configuration = configurationManager;
            // Read host, port, and default index from appsettings
            var host = configuration["Elasticsearch:Host"];
            var port = configuration["Elasticsearch:Port"];
            var defaultIndex = configuration["Elasticsearch:Index"];
            if (defaultIndex == null)
                throw new Exception("Default index is missing in the configuration");

            // Construct the URI for Elasticsearch
            var elasticUri = $"{host}:{port}";

            // Create Elasticsearch client settings
            var settings = new ElasticsearchClientSettings(new Uri(elasticUri))
                .DefaultIndex(defaultIndex); // Set the default index from configuration
            var searchClient = new ElasticsearchClient(settings);
            Task.Run(() => CreateProductMapping(searchClient));
            return searchClient;
        });
    }

    public static void RegisterDbServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProductService, ProductService>();
        serviceCollection.AddScoped<IProductItemService, ProductItemService>();
    }

    private static async Task CreateProductMapping(ElasticsearchClient elasticClient)
{
    var indexExistsResponse = await elasticClient.Indices.ExistsAsync("products");

    if (!indexExistsResponse.Exists)
    {
        // Index does not exist, create it
        var createIndexResponse = await elasticClient.Indices.CreateAsync("products", c => c
            .Mappings(m => m
                .Properties(new Properties<Product>
                {
                    // Mapping for Product fields
                    { "name", new TextProperty() }, // Name field
                    { "brand", new TextProperty() }, // Brand field
                    { "category", new TextProperty() }, // Category field
                    { "description", new TextProperty() }, // Description field
                    { "items", new NestedProperty() // Items as Object
                        {
                            // Nested properties inside Items
                            Properties = new Properties<ProductItemEventModel>
                            {
                                { "name", new TextProperty() }, // Name inside Item
                                { "minPrice", new DoubleNumberProperty() }, // MinPrice inside Item
                                { "maxPrice", new DoubleNumberProperty() }, // MaxPrice inside Item
                                { "productId", new KeywordProperty() }, // ProductId inside Item
                                { "id", new KeywordProperty() }, // Id inside Item
                                { "attributes", new TextProperty() } // Attributes inside Item
                            }
                        }
                    }
                })
            )
        );

        Console.WriteLine(
            !createIndexResponse.IsValidResponse
                ? $"Failed to create index: {createIndexResponse.ElasticsearchServerError?.Error.Reason}"
                : "Index created successfully.");
    }
    else
    {
        Console.WriteLine("Index already exists.");
    }
}

}