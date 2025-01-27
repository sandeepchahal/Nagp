using Confluent.Kafka;
using ProductAPI.Models;

namespace ProductAPI.Events;

public class ProductEvent:IProductEvent
{
    public async Task RaiseAddProductAsync(Product product)
    {
        // Produce Kafka message
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        var productJson = System.Text.Json.JsonSerializer.Serialize(product);

        var result = await producer.ProduceAsync("product-topic", new Message<Null, string>
        {
            Value = productJson
        });

    }
}