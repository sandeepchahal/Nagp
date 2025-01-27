using System.Text.Json;
using Confluent.Kafka;
using Elastic.Clients.Elasticsearch;
using SearchAPI.Models;

namespace SearchAPI.Tasks;

public class ProductConsumer:BackgroundService
{
    private readonly ElasticsearchClient _elasticClient;

    public ProductConsumer(ElasticsearchClient elasticClient)
    {
        _elasticClient = elasticClient;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "search-api-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Null, string>(config).Build();
        consumer.Subscribe("product-topic");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);

                var product = JsonSerializer.Deserialize<Product>(result.Message.Value);
                if (product != null)
                {
                    // Index product into Elasticsearch
                    var response = await _elasticClient.IndexAsync(product, idx => idx.Index("products"));
                    if (!response.IsValidResponse)
                    {
                        Console.WriteLine($"Failed to index product: {response.DebugInformation}");
                    }
                }
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error occurred: {e.Error.Reason}");
            }
        }
    }
}