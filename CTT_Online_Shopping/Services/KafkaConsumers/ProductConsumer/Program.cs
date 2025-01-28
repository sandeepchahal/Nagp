using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductConsumer.DbServices;
using ProductConsumer.Services;

var builder = Host.CreateApplicationBuilder(args);

// Read configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

// Register background service
builder.Services.AddHostedService<ProductConsumerService>();


builder.Services.AddSingleton<ElasticsearchClient>(_ =>
{
    var configuration = builder.Configuration;

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

builder.Services.AddScoped<IProductService, ProductService>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // or other logging providers

// Build the host
var app = builder.Build();

// Run the application
await app.RunAsync();