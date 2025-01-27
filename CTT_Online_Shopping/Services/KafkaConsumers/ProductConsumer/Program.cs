using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductConsumer.Services;

var builder = Host.CreateApplicationBuilder(args);

// Read configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Register background service
builder.Services.AddHostedService<ProductConsumerService>();

// Build the host
var app = builder.Build();

// Run the application
await app.RunAsync();