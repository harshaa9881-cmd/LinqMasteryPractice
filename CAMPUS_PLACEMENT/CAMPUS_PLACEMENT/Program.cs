using CampusPlacementManagement.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers

builder.Services.AddControllers();

// Database Configuration

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// OpenAPI Configuration

builder.Services.AddOpenApi();

// Authorization

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure HTTP Request Pipeline

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();