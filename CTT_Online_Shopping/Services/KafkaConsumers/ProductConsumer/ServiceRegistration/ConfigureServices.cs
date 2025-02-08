using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Transport;
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

            var cloudUrl = configuration["ElasticSearch:CloudUrl"]; // Update: Cloud URL instead of CloudId
            var apiKey = configuration["ElasticSearch:api_key"];
            var defaultIndex = configuration["ElasticSearch:Index"];

            if (string.IsNullOrEmpty(cloudUrl))
                throw new Exception("Elasticsearch CloudUrl is missing in the configuration");

            if (string.IsNullOrEmpty(apiKey))
                throw new Exception("Elasticsearch API Key is missing in the configuration");

            if (string.IsNullOrEmpty(defaultIndex))
                throw new Exception("Default index is missing in the configuration");

            // Create Elasticsearch settings using Cloud URL and API Key
            var settings = new ElasticsearchClientSettings(new Uri(cloudUrl))
                .Authentication(new ApiKey(apiKey)) // API Key Authentication
                .DefaultIndex(defaultIndex);

            var client = new ElasticsearchClient(settings);
            Task.Run(() => CreateProductMapping(client));
            return client;
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