using api.Data;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Repository;
using api.Models;
using api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IFoodEntryRepository, FoodEntryRepository>();
builder.Services.AddScoped<INutritionCalculator, NutritionCalculator>();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services
    .AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<AppUser>();
app.MapControllers();

app.Run();
