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
    private readonly IProductService _productService;
    public ProductConsumerService(ILogger<ProductConsumerService> logger, IConfiguration configuration, IProductService productService)
    {
        var host = configuration["Kafka:Host"];
        var port = configuration["Kafka:Port"];
        var groupId = configuration["Kafka:GroupId"];
        _productService = productService;

        var config = new ConsumerConfig
        {
            BootstrapServers = $"{host}:{port}",
            GroupId = $"{groupId}-product", 
            AutoOffsetReset = AutoOffsetReset.Earliest,
            SecurityProtocol = SecurityProtocol.Plaintext
        };
        _consumer = new ConsumerBuilder<Null, string>(config).Build();
        _consumer.Subscribe(Topic);
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    var productConsumerModel = JsonSerializer.Deserialize<ProductConsumerModel>(result.Message.Value);
                    if (productConsumerModel == null) continue;
                    if (productConsumerModel.EventType == null)
                    {
                        _logger.LogInformation("Event Type is null");
                        continue;
                    }
                    if (productConsumerModel.Product == null)
                    {
                        _logger.LogInformation("ProductEventModel is null");
                        continue;
                    }

                    _ = productConsumerModel.EventType == "Add"
                        ? HandleAdd(productConsumerModel.Product)
                        : HandleUpdate(productConsumerModel.Product);
                    _consumer.Commit(result);
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"Error consuming message: {e.Error.Reason}");
                }
            }
        }, stoppingToken);
    }
    private Task HandleAdd(ProductEventModel productEventModel)
    {
        _logger.LogInformation($"Handling Add Product Event Type \n: {productEventModel}");
        _ = _productService.Add(productEventModel: productEventModel);
        return Task.CompletedTask;
    }
    private Task HandleUpdate(ProductEventModel productEventModel)
    {
        _logger.LogInformation($"Handling Update Product Event Type \n: {productEventModel}");
        _ = _productService.Update(productEventModel.Id!,productEventModel);
        return Task.CompletedTask;

    }

}
