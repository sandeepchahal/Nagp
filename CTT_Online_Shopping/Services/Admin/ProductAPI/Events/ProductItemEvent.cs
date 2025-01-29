using Confluent.Kafka;
using ProductAPI.Events.Models;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Events;

public class ProductItemEvent:IProductItemEvent
{
    private const string Topic = "ctt-product-item";
    private readonly ProducerConfig _config;

    public ProductItemEvent(IConfiguration configuration)
    {
        var host = configuration["Kafka:Host"];
        var port = configuration["Kafka:Port"];

        _config = new ProducerConfig
        {
            BootstrapServers = $"{host}:{port}"
        };
    }
    public async Task RaiseAddAsync(ProductItemDb productItemDb)
    {
        try
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            var productItemEventModel = new ProductItemEventModel()
            {
                EventType = "Add",
                ProductItem = productItemDb
            };
            var productItemJson = System.Text.Json.JsonSerializer.Serialize(productItemEventModel);

            var result = await producer.ProduceAsync(Topic, new Message<Null, string>
            {
                Value = productItemJson
            });

            // Log result if necessary
            Console.WriteLine($"Message delivered to {result.TopicPartitionOffset}");
        }
        catch (Exception ex)
        {
            // Log exception for debugging
            Console.Error.WriteLine($"Error producing Kafka message: {ex.Message}");
            throw; // Rethrow the exception if needed
        }
    }

    public async Task RaiseUpdateAsync(ProductItemDb productItemDb)
    {
        try
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            var productItemEventModel = new ProductItemEventModel()
            {
                EventType = "Update",
                ProductItem = productItemDb
            };
            var productItemJson = System.Text.Json.JsonSerializer.Serialize(productItemEventModel);

            var result = await producer.ProduceAsync(Topic, new Message<Null, string>
            {
                Value = productItemJson
            });

            // Log result if necessary
            Console.WriteLine($"Message delivered to {result.TopicPartitionOffset}");
        }
        catch (Exception ex)
        {
            // Log exception for debugging
            Console.Error.WriteLine($"Error producing Kafka message: {ex.Message}");
            throw; // Rethrow the exception if needed
        } 
    }
}