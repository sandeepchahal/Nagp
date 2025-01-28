using Confluent.Kafka;
using ProductAPI.Events.Models;
using ProductAPI.Models;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Events;

public class ProductEvent(IConfiguration configuration):IProductEvent
{
    private const string Topic = "ctt-product";

    public async Task RaiseAddProductAsync(ProductDb productDb)
    {
        try
        {
            var host = configuration["Kafka:Host"];
            var port = configuration["Kafka:Port"];

            var config = new ProducerConfig
            {
                BootstrapServers = $"{host}:{port}"
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var productEventModel = new ProductEventModel()
            {
                EventType = "Add",
                Product = productDb
            };
            var productJson = System.Text.Json.JsonSerializer.Serialize(productEventModel);

            var result = await producer.ProduceAsync(Topic, new Message<Null, string>
            {
                Value = productJson
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