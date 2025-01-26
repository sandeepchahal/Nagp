using Elastic.Clients.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ElasticsearchClient>(sp =>
{
    var configuration = builder.Configuration;

    // Read host, port, and default index from appsettings
    var host = configuration["Elasticsearch:Host"];
    var port = configuration["Elasticsearch:Port"];
    var defaultIndex = configuration["Elasticsearch:DefaultIndex"];

    // Construct the URI for Elasticsearch
    var elasticUri = $"{host}:{port}";

    // Create Elasticsearch client settings
    var settings = new ElasticsearchClientSettings(new Uri(elasticUri))
        .DefaultIndex(defaultIndex); // Set the default index from configuration

    return new ElasticsearchClient(settings);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();