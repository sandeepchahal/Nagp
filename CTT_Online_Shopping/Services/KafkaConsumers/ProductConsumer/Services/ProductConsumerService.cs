using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductConsumer.DbServices;
using ProductConsumer.Models;

namespace ProductConsumer.Services;

public class ProductConsumerService : BackgroundService
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly ILogger<ProductConsumerService> _logger;
    private const string Topic = "ctt-product";
    private IProductService _productService;
    public ProductConsumerService(ILogger<ProductConsumerService> logger, IConfiguration configuration, IProductService productService)
    {
        var host = configuration["Kafka:Host"];
        var port = configuration["Kafka:Port"];
        var groupId = configuration["Kafka:GroupId"];
        _productService = productService;

        var config = new ConsumerConfig
        {
            BootstrapServers = $"{host}:{port}",
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Null, string>(config).Build();
        _consumer.Subscribe(Topic);
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(stoppingToken);
                var productConsumerModel = JsonSerializer.Deserialize<ProductConsumerModel>(result.Message.Value);

                if (productConsumerModel == null) continue;
                if (productConsumerModel.EventType == "Add")
                {
                    _ = HandleAdd(productConsumerModel.Product);
                }
                else
                {
                    HandleUpdate(productConsumerModel.Product);
   
                }
            }
            catch (ConsumeException e)
            {
                _logger.LogError($"Error consuming message: {e.Error.Reason}");
            }
        }
    }

    private Task HandleAdd(Product product)
    {
        _logger.LogInformation($"Handling Add Event Type \n: {product}");
        _ = _productService.Add(product: product);
        return Task.CompletedTask;
    }
    private void HandleUpdate(Product product)
    {
        _logger.LogInformation($"Handling Update Event Type \n: {product}");

    }

}
