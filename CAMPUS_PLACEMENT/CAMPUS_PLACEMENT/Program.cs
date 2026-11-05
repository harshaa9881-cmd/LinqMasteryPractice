using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Repositories.Implementations;
using CampusPlacementManagement.Services.Interfaces;
using CampusPlacementManagement.Services.Implementations;

using System.Security.Claims;
using System.Text;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers

builder.Services.AddControllers();

// Database Configuration

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Identity Configuration

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Repository Registration

builder.Services.AddScoped<IUserRepository, UserRepository>();

// Service Registration

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IJwtService, JwtService>();


builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();

builder.Services.AddScoped<IPlacementOfficerRepository, PlacementOfficerRepository>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddScoped<ICompanyHRRepository, CompanyHRRepository>();


builder.Services.AddScoped<ICollegeService, CollegeService>();
builder.Services.AddScoped<IPlacementOfficerService, PlacementOfficerService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyHRService, CompanyHRService>();


// JWT Configuration

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT signing key is not configured."
    );
}

var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtKey)
);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultForbidScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],

                IssuerSigningKey = key,

                ClockSkew = TimeSpan.Zero,

                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(
                    "JWT Authentication Failed: " +
                    context.Exception.Message
                );

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine(
                    "JWT Token Validated Successfully"
                );

                return Task.CompletedTask;
            }
        };
    });

// Authorization

builder.Services.AddAuthorization();

// OpenAPI

builder.Services.AddOpenApi();

var app = builder.Build();

// Seed Roles and Initial SuperAdmin

await RoleSeeder.SeedAsync(
    app.Services,
    app.Configuration
);

// HTTP Request Pipeline

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();