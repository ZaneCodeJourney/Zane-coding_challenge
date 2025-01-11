using LibraryManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel with a different port
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5051, listenOptions => listenOptions.UseHttps()); // HTTPS
});

// Configure InMemoryDatabase
builder.Services.AddDbContext<LibraryContext>(options => options.UseInMemoryDatabase("LibraryDb"));

// Register controller services
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Library API", Version = "v1" }
    );
});

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin() // Allow any origin
                .AllowAnyMethod() // Allow any HTTP method
                .AllowAnyHeader(); // Allow any header
        }
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1"));
}

// Enable CORS
app.UseCors("AllowAll");

// Configure authorization (no complex authentication logic required here)
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Initialize seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

app.Run();
