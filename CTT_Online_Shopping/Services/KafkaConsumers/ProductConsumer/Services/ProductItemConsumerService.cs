using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductConsumer.DbServices;
using ProductConsumer.Models;

namespace ProductConsumer.Services;

public class ProductItemConsumerService : BackgroundService
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly ILogger<ProductItemConsumerService> _logger;
    private const string Topic = "ctt-product-item";
    private readonly IProductItemService _productItemService;

    public ProductItemConsumerService(ILogger<ProductItemConsumerService> logger, IConfiguration configuration, IProductItemService productItemService)
    {
        var host = configuration["Kafka:Host"];
        var port = configuration["Kafka:Port"];
        var groupId = configuration["Kafka:GroupId"];
        _productItemService = productItemService;

        var config = new ConsumerConfig
        {
            BootstrapServers = $"{host}:{port}",
            GroupId = $"{groupId}-product-item", // Different group ID to avoid conflicts
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
                    var productItemModel = JsonSerializer.Deserialize<ProductItemConsumerModel>(result.Message.Value);

                    if (productItemModel == null) continue;

                    _ = productItemModel.EventType == "Add"
                        ? HandleAdd(productItemModel.ProductItem)
                        : HandleUpdate(productItemModel.ProductItem);
                    _consumer.Commit(result);
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"Error consuming ProductItem message: {e.Error.Reason}");
                }
            }
        }, stoppingToken);
    }

    private Task HandleAdd(ProductItemEventModel productItemEventModel)
    {
        _logger.LogInformation($"Handling ProductItem Add Event \n: {productItemEventModel}");
        _ = _productItemService.Add(productItemEventModel);
        return Task.CompletedTask;
    }

    private Task HandleUpdate(ProductItemEventModel productItemEventModel)
    {
        _logger.LogInformation($"Handling ProductItem Update Event \n: {productItemEventModel}");
        _ = _productItemService.Update(productItemEventModel.Id!, productItemEventModel);
        return Task.CompletedTask;
    }
}