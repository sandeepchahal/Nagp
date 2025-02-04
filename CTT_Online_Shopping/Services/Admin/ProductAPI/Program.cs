using MongoDB.Bson.Serialization.Conventions;
using ProductAPI.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.ConfigureMongoDb(builder.Configuration);
builder.Services.AddScopedServices();
builder.Services.ConfigureDbServices();

// Register MongoDB camelCase convention
var conventionPack = new ConventionPack
{
    new CamelCaseElementNameConvention()
};
ConventionRegistry.Register("CamelCaseConvention", conventionPack, _ => true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add CORS middleware
app.UseCors("AllowAll");  // Use the CORS policy you've configured

app.MapControllers();
app.Run();
