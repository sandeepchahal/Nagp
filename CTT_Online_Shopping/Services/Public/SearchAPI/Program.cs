using SearchAPI.ServiceConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add Swagger only in Development environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
    builder.Services.AddOpenApi();
    
}

await builder.Services.RegisterElasticClientServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true) // ✅ Allow all origins dynamically
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Request received at {Path} with method {Method}", context.Request.Path, context.Request.Method);
    
    await next(); // Call the next middleware
});

app.UseCors("AllowAll");
app.MapControllers();
app.Run();