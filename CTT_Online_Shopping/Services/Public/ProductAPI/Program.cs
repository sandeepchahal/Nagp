using MongoDB.Bson.Serialization.Conventions;
using ProductAPI.ServiceRegistrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
    builder.Services.AddOpenApi();
}

builder.Services.ConfigureMongoDb(builder.Configuration);
builder.Services.ConfigureDbServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true) // ✅ Allow all origins dynamically
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Logging.AddConsole();

var conventionPack = new ConventionPack
{
    new CamelCaseElementNameConvention()
};
ConventionRegistry.Register("CamelCaseConvention", conventionPack, _ => true);


var app = builder.Build();
app.UseCors("AllowAll"); 

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting(); 
app.Use(async (context, next) =>
{
    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("RequestLogger");

    logger.LogInformation("Request received at {Path} with method {Method}", context.Request.Path, context.Request.Method);
    
    await next(); 
});
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
        return;
    }
    await next();
});
app.MapControllers();
app.Run();