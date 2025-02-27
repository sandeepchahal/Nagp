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
        var elasticClient = await ConfigureConnection(configurationManager);
        serviceCollection.AddSingleton(elasticClient);
    }

    private static async Task<ElasticsearchClient> ConfigureConnection(ConfigurationManager configurationManager)
    {
        var cloudUrl = configurationManager["ElasticSearch:CloudUrl"];
        var username = configurationManager["ElasticSearch:Username"];
        var password = configurationManager["ElasticSearch:Password"];
        var defaultIndex = configurationManager["ElasticSearch:Index"];
        if (string.IsNullOrEmpty(defaultIndex))
            throw new Exception("Default index is missing in the configuration");

        if (string.IsNullOrEmpty(cloudUrl))
            throw new Exception("Elasticsearch URL is missing in the configuration");

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            throw new Exception("Elasticsearch username or password is missing");

        var settings = new ElasticsearchClientSettings(new Uri(cloudUrl))
            .Authentication(new BasicAuthentication(username, password)).DefaultIndex(defaultIndex);


        var client = new ElasticsearchClient(settings);

        // Ping the Elasticsearch cluster to check the connection
        var pingResponse = await client.PingAsync();
        if (!pingResponse.IsValidResponse)
        {
            throw new Exception(
                "Failed to connect to Elasticsearch cluster. Check your credentials and network settings.");
        }

        Console.WriteLine("Pined db successfully");
        _ = Task.Run(() => CreateProductMapping(client));
        
        return client;
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
                .Properties(new Properties<ProductItemEventModel>
                {
                    // Mapping for Product fields
                    { "productItemId", new KeywordProperty() }, // ProductItemId
                    { "productId", new KeywordProperty() }, // ProductId
                    { "name", new TextProperty() }, // Name field
                    { "brand", new TextProperty() }, // Brand field
                    { "gender", new TextProperty() }, // Gender field
                    { "subCategoryName", new TextProperty() }, // SubCategoryName field
                    { "subCategoryId", new KeywordProperty() }, // SubCategoryId field
                    { "subCategorySlug", new KeywordProperty() }, // SubCategorySlug field
                    { "variantType", new TextProperty() }, // VariantType field
                    { "minPrice", new DoubleNumberProperty() }, // MinPrice field
                    { "maxPrice", new DoubleNumberProperty() }, // MaxPrice field

                    // Nested properties for lists
                    { "sizeVariant", new NestedProperty
                        {
                            Properties = new Properties<ProductVariantSizeEventModel>
                            {
                                { "sizeId", new KeywordProperty() }, // SizeId
                                { "size", new TextProperty() } // Size
                            }
                        }
                    }, // SizeVariant list field
                    
                    { "colorVariant", new NestedProperty
                        {
                            Properties = new Properties<ProductVariantColorEventModel>
                            {
                                { "colorId", new KeywordProperty() }, // ColorId
                                { "color", new TextProperty() } // Color
                            }
                        }
                    }, // ColorVariant list field
                    
                    { "sizeColorVariant", new NestedProperty
                        {
                            Properties = new Properties<ProductVariantSizeAndColorEventModel>
                            {
                                { "sizeAndColorId", new KeywordProperty() }, // SizeAndColorId
                                { "color", new ObjectProperty
                                    {
                                        Properties = new Properties<ProductVariantColorEventModel>
                                        {
                                            { "colorId", new KeywordProperty() }, // ColorId
                                            { "color", new TextProperty() } // Color
                                        }
                                    }
                                }, // Nested Color object
                                { "sizes", new NestedProperty
                                    {
                                        Properties = new Properties<ProductVariantSizeEventModel>
                                        {
                                            { "sizeId", new KeywordProperty() }, // SizeId
                                            { "size", new TextProperty() } // Size
                                        }
                                    }
                                } // Nested Sizes list
                            }
                        }
                    } // SizeColorVariant list field
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