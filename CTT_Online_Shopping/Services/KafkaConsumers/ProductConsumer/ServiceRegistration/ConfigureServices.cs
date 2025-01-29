using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductConsumer.DbServices;
using ProductConsumer.Services;

namespace ProductConsumer.ServiceRegistration;

public static class ConfigureServices
{
    public static void RegisterBackgroundServices(this IServiceCollection serviceCollection)
    {
        // Register background service
        serviceCollection.AddHostedService<ProductConsumerService>();
        serviceCollection.AddHostedService<ProductItemConsumerService>();
    }
    public static void RegisterElasticClientServices(this IServiceCollection serviceCollection, ConfigurationManager configurationManager)
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

            return new ElasticsearchClient(settings);
        });

    }

    public static void RegisterDbServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProductService, ProductService>();
        serviceCollection.AddScoped<IProductItemService, ProductItemService>();
    }
}