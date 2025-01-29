using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductConsumer.ServiceRegistration;

var builder = Host.CreateApplicationBuilder(args);

// Read configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

// Register services
builder.Services.RegisterDbServices();
builder.Services.RegisterElasticClientServices(builder.Configuration);
builder.Services.RegisterBackgroundServices();

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // or other logging providers

// Build the host
var app = builder.Build();

// Run the application
await app.RunAsync();