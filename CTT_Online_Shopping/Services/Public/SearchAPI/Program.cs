using Elastic.Clients.Elasticsearch;
using Microsoft.IdentityModel.Protocols.Configuration;
using SearchAPI.ServiceConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
await builder.Services.RegisterElasticClientServices(builder.Configuration);


// Allowing all origins for testing purposes (you can restrict to specific domains later)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // You can also specify a domain like "https://yourfrontend.com"
            .AllowAnyMethod()  // You can also specify the allowed HTTP methods like .AllowGet(), .AllowPost()...
            .AllowAnyHeader(); // You can also specify headers if needed
    });
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
app.UseCors("AllowAll");
app.MapControllers();
app.Run();