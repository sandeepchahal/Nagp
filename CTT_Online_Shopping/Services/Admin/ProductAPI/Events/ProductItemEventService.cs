using Confluent.Kafka;
using ProductAPI.Events.Models;

namespace ProductAPI.Events;

public class ProductItemEventService:IProductItemEventService
{
    private const string Topic = "ctt-product-item";
    private readonly ProducerConfig _config;

    public ProductItemEventService(IConfiguration configuration)
    {
        var host = configuration["Kafka:Host"];
        var port = configuration["Kafka:Port"];

        _config = new ProducerConfig
        {
            BootstrapServers = $"{host}:{port}"
        };
    }
    public async Task RaiseAddAsync(ProductItemEventModel productItemEventModel)
    {
        try
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            var eventModel = new ProductItemRaiseEventModel()
            {
                EventType = "Add",
                ProductItem = productItemEventModel
            };
            var productItemJson = System.Text.Json.JsonSerializer.Serialize(eventModel);

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

    public async Task RaiseUpdateAsync(ProductItemEventModel productItemEventModel)
    {
        try
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            var eventModel = new ProductItemRaiseEventModel()
            {
                EventType = "Update",
                ProductItem = productItemEventModel
            };
            var productItemJson = System.Text.Json.JsonSerializer.Serialize(eventModel);

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