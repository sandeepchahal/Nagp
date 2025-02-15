using MongoDB.Bson.Serialization.Conventions;
using ProductAPI.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
            .WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader();
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

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();
app.UseCors("AllowAll");  // Use the CORS policy you've configured

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Request received at {Path} with method {Method}", context.Request.Path, context.Request.Method);
    
    await next(); // Call the next middleware
});
app.UseHttpsRedirection();


app.MapControllers();
app.Run();
