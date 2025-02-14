using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserAPI.DbContext;
using UserAPI.ServiceRegistrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add Swagger only in Development environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
    builder.Services.AddOpenApi();
}

builder.Services.ConfigureDbServices();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

// Configure password policy and other options (optional)
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
});

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
ConfigureMigration.ConfigureMigrationServices(app);

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